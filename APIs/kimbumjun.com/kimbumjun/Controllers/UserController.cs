using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using kimbumjun.BindingModel;
using kimbumjun.DTO;
using kimbumjun.Enums;
using kimbumjun.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace kimbumjun.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController>   _logger;
    private readonly UserManager<AppUser>      _userManager;
    private readonly SignInManager<AppUser>    _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly JWTConfig                 _jwtConfig;

    public UserController(ILogger<UserController> logger,
        UserManager<AppUser>                      userManager,
        SignInManager<AppUser>                    signInManager,
        IOptions<JWTConfig>                       jwtConfig,
        RoleManager<IdentityRole>                 roleManager)
    {
        _userManager   = userManager;
        _signInManager = signInManager;
        _roleManager   = roleManager;
        _logger        = logger;
        _jwtConfig     = jwtConfig.Value;
    }

    /// <summary>
    /// 회원등록
    /// https://localhost:8947/api/user/RegisterUser
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("RegisterUser")]
    public async Task<IActionResult> RegisterUser([FromBody] AddUpdateRegisterUserBindingModel model)
    {
        try
        {
            if (!await _roleManager.RoleExistsAsync(model.Role))
            {
                return BadRequest(new ResponseModel(ResponseCode.Error, "Role does not exist", model));
            }

            if (_userManager.Users.Any(x => x.Email.Equals(model.Email)))
            {
                return BadRequest(new ResponseModel(ResponseCode.Error, "이미등록된 회원입니다.", model));
            }

            var user = new AppUser
            {
                FullName     = model.FullName,
                Email        = model.Email,
                UserName     = model.Email, // 매우중요
                DateCreated  = DateTime.UtcNow,
                DateModified = DateTime.UtcNow
            };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new ResponseModel(ResponseCode.Error, "회원등록에 실패하였습니다.",
                    result.Errors.Select(x => x.Description).ToArray()));
            }

            var tempUser = await _userManager.FindByEmailAsync(model.Email);

            await _userManager.AddToRoleAsync(tempUser, model.Role);

            return Ok(new ResponseModel(ResponseCode.OK, "회원등록이 완료되었습니다.", model));
        }
        catch (Exception ex)
        {
            return BadRequest(new ResponseModel(ResponseCode.Error, ex.Message, null));
        }
    }

    /// <summary>
    /// 회원정보 수정 (fullName, password, role)
    /// https://localhost:8947/api/user/updateUser
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("UpdateUser")]
    public async Task<IActionResult> UpdateUser([FromBody] AddUpdateRegisterUserBindingModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        AppUser user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
            return NotFound(model);

        user.FullName     = model.FullName;
        user.DateModified = DateTime.UtcNow;

        string password = _userManager.PasswordHasher.HashPassword(user, model.Password);
        user.PasswordHash = password;

        var roles = await _userManager.GetRolesAsync(user);
        if (roles.Any())
        {
            foreach (var role in roles)
            {
                await _userManager.RemoveFromRoleAsync(user, role);
            }
        }

        await _userManager.AddToRoleAsync(user, model.Role);

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            return BadRequest(new ResponseModel(ResponseCode.Error, "회원정보 수정에 실패하였습니다.",
                result.Errors.Select(x => x.Description.ToArray())));
        }

        return Ok($"회원 ({user}) 정보수정이 완료 되었습니다.\n{password}");
    }

    /// <summary>
    /// 롤 추가
    /// https://localhost:8947/api/user/addUpdateRole
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("AddUpdateRole")]
    public async Task<IActionResult> AddUpdateRole([FromBody] AddUpdateRegisterUserBindingModel model)
    {
        AppUser user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null)
            return NotFound($"{model.FullName} 의 이메일 {model.Email} 정보가 잘못되었습니다.");

        bool check = await _userManager.IsInRoleAsync(user, model.Role);

        if (check) return Ok($"요청하신 Role: ({model.Role}) 은 이미 있습니다.");

        IdentityResult result = await _userManager.AddToRoleAsync(user, model.Role);

        if (result.Succeeded)
            return Ok($"{(await _userManager.GetRolesAsync(user)).FirstOrDefault()} Role 추가완료 ");

        return BadRequest($"Role: {model.Role} 추가 작업에 실패하였습니다.");
    }

    /// <summary>
    /// 전체 회원정보 가져오기
    /// api/usr/getalluser
    /// </summary>
    /// <returns></returns>
    [Authorize(Roles = "Admin")]
    [HttpGet("GetAllUser")]
    public async Task<object> GetAllUser()
    {
        try
        {
            var allUserDTO = new List<UserDTO>();
            var users      = _userManager.Users.ToList();
            if (!users.Any()) return NotFound();

            foreach (var user in users)
            {
                var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
                // string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                allUserDTO.Add(new UserDTO(user.FullName, user.Email, user.UserName, user.DateCreated, role));
            }

            return await Task.FromResult(allUserDTO);
            // return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", allUserDTO));
        }
        catch (Exception ex)
        {
            return await Task.FromResult(ex.Message);
        }
    }

    /// <summary>
    /// Get User List Only User Role
    /// https://localhost:8947/api/user/getuser
    /// Role 이 User 만접근 가능
    /// </summary>
    /// <returns></returns>
    [Authorize(Roles = "Admin,User")]
    [HttpGet("GetUser")]
    public async Task<object> GetUser()
    {
        try
        {
            var allUserDTO = new List<UserDTO>();
            var users      = _userManager.Users.ToList();
            if (!users.Any()) return NotFound();
            foreach (var user in users)
            {
                var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
                if (role == "User")
                {
                    allUserDTO.Add(new UserDTO(user.FullName, user.Email, user.UserName, user.DateCreated, role));
                }
            }

            // return await Task.FromResult(allUserDTO);
            return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", allUserDTO));
        }
        catch (Exception ex)
        {
            return await Task.FromResult(ex.Message);
        }
    }

    /// <summary>
    /// https://localhost:8947/api/user/RegisterUser
    /// api/user/login
    /// </summary>
    /// <param name="model"></param>
    [HttpPost("Login")]
    public async Task<object> Login([FromBody] LoginBindingModel model)
    {
        try
        {
            if (!ModelState.IsValid)
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Invalid Email or password", null));

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);


            if (!result.Succeeded)
                return await Task.FromResult($"{model.Email}, {model.Password} invalid email or password");

            var appUser = await _userManager.FindByEmailAsync(model.Email);

            var role = (await _userManager.GetRolesAsync(appUser)).FirstOrDefault();

            var user = new UserDTO(appUser.FullName, appUser.Email, appUser.UserName, appUser.DateCreated, role)
            {
                // 토큰 생성
                Token = GenerateToken(appUser, role)
            };

            return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Login successed!", user));
        }
        catch (Exception ex)
        {
            return Task.FromResult(ex.Message);
        }
    }

    /// <summary>
    /// Add Role
    /// https://localhost:8947/api/user/AddRole
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [Authorize(Roles = "Admin")]
    [HttpPost("AddRole")]
    public async Task<object> AddRole([FromBody] AddRoleBindingModel model)
    {
        try
        {
            if (model == null || string.IsNullOrEmpty(model.Role))
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "parameters are missing", model));
            }

            if (await _roleManager.RoleExistsAsync(model.Role))
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Role already exist", null));
            }

            var role = new IdentityRole
            {
                Name = model.Role
            };
            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Role added successfully", null));
            }

            return await Task.FromResult(new ResponseModel(ResponseCode.Error,
                "something went wrong please try again later", null));
        }
        catch (Exception ex)
        {
            return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
        }
    }


    [Authorize(Roles = "Admin")]
    [HttpGet("GetRoles")]
    public async Task<object> GetRoles()
    {
        try
        {
            var roles = _roleManager.Roles.ToList();
            if (!roles.Any()) return NotFound();
            return await Task.FromResult(roles.Select(x => x.Name));
        }
        catch (Exception ex)
        {
            return await Task.FromResult(ex.Message);
        }
    }

    /// <summary>
    /// 비밀번호 확인 (비번 변경전 확인용)
    /// https://localhost:8947/api/user/checkpassword
    /// </summary>
    /// <param name="model">email, old password</param>
    /// <returns></returns>
    [HttpPost("CheckPassword")]
    public async Task<IActionResult> CheckPassword([FromBody] LoginBindingModel model)
    {
        AppUser user = await _userManager.FindByEmailAsync(model.Email);
        PasswordVerificationResult check =
            _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);

        return Ok(check == PasswordVerificationResult.Success 
            ? $"{user.PasswordHash} - {model.Password} 비밀번호 동일함" 
            : $"{user.PasswordHash} - {model.Password} 비밀번호가 맞지 않음");
    }

    /// <summary>
    /// 토큰생성
    /// </summary>
    /// <param name="user"></param>
    /// <param name="role"></param>
    /// <returns></returns>
    private string GenerateToken(AppUser user, string role)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var key             = Encoding.ASCII.GetBytes(_jwtConfig.Key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, role)
            }),
            Expires = DateTime.UtcNow.AddHours(12),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = _jwtConfig.Audience,
            Issuer   = _jwtConfig.Issuer
        };

        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        return jwtTokenHandler.WriteToken(token);
    }
}

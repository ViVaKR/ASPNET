using System.Security.Claims;
using System.Security.Cryptography.Pkcs;
using System.Text;
using kimbumjun.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace kimbumjun.Common;

public class Utils
{
    public static string Encrypt(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public static bool Verify(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }

    public static string GenerateJWTToken(long userId, bool rememberMe)
    {
        IdentityOptions option = new IdentityOptions();
        
        // // var key = Encoding.ASCII.GetBytes(.Secret);
        // // We will setup descripbe the parameters of token generation
        // var tokenDescription = new SecurityTokenDescriptor
        // {
        //     Subject = new ClaimsIdentity(new Claim[]
        //     {
        //         new Claim("userId", userId.ToString())
        //     }),
        //     Expires = rememberMe ? DateTime.UtcNow.AddDays(7) : DateTime.UtcNow.AddDays(1),
        //     SigningCredentials =
        //         new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //     // SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Properties.Resource)))
        // };

        // var token = tokenHandler.CreateToken(tokenDescriptor);
        // return tokenHandler.WriteToken(token);

        return null;
    }
}

using Microsoft.Build.Framework;

namespace kimbumjun.BindingModel;

public class LoginBindingModel
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}

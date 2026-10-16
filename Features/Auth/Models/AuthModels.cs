using System.ComponentModel.DataAnnotations;
using BarberHub.Client.Core.Models;

namespace BarberHub.Client.Features.Auth.Models;

// Contracts/Auth/LoginRequest.cs : Username, Password, Role
public sealed class LoginRequest
{
    [Required(ErrorMessage = "نام کاربری الزامی است")]
    public string Username { get; set; } = "";

    [Required(ErrorMessage = "رمز عبور الزامی است")]
    public string Password { get; set; } = "";

    public UserRole Role { get; set; } = UserRole.EndUser;
}

// Contracts/Auth/RegisterRequest.cs
public sealed class RegisterRequest
{
    [Required(ErrorMessage = "نام الزامی است")] public string FirstName { get; set; } = "";
    [Required(ErrorMessage = "نام خانوادگی الزامی است")] public string LastName { get; set; } = "";
    [Required(ErrorMessage = "نام کاربری الزامی است")] public string Username { get; set; } = "";

    [Required(ErrorMessage = "رمز عبور الزامی است")]
    [MinLength(6, ErrorMessage = "رمز عبور حداقل ۶ کاراکتر باشد")]
    public string Password { get; set; } = "";

    [Required(ErrorMessage = "شماره موبایل الزامی است")]
    [RegularExpression(@"^09\d{9}$", ErrorMessage = "شماره موبایل را به شکل 09xxxxxxxxx وارد کنید")]
    public string MobileNumber { get; set; } = "";
}

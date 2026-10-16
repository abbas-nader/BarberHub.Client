using System.ComponentModel.DataAnnotations;

namespace BarberHub.Client.Features.Account.Models;

public sealed record UserResponse(long Id, string FirstName, string LastName, string UserName, string? MobileNumber, bool? IsMobileVerified);

public sealed record EndUserResponse(long Id, string FirstName, string LastName, string UserName, string? MobileNumber, bool? IsMobileVerified);

public sealed class ChangePasswordRequest
{
    [Required(ErrorMessage = "رمز فعلی الزامی است")] public string OldPassword { get; set; } = "";
    [Required(ErrorMessage = "رمز جدید الزامی است")]
    [MinLength(6, ErrorMessage = "رمز جدید حداقل ۶ کاراکتر باشد")]
    public string NewPassword { get; set; } = "";
}

public sealed class UpdateEndUserRequest
{
    [Required(ErrorMessage = "نام الزامی است")] public string FirstName { get; set; } = "";
    [Required(ErrorMessage = "نام خانوادگی الزامی است")] public string LastName { get; set; } = "";
    [Required(ErrorMessage = "نام کاربری الزامی است")] public string UserName { get; set; } = "";
    [Required(ErrorMessage = "شماره موبایل الزامی است")]
    [RegularExpression(@"^09\d{9}$", ErrorMessage = "شماره موبایل را به شکل 09xxxxxxxxx وارد کنید")]
    public string MobileNumber { get; set; } = "";
}

public sealed class VerifyOtpRequest
{
    [Required(ErrorMessage = "کد را وارد کنید")] public string Code { get; set; } = "";
}

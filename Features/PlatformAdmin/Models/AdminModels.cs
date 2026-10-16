using System.ComponentModel.DataAnnotations;

namespace BarberHub.Client.Features.PlatformAdmin.Models;

public sealed record SalonAdminResponse(long Id, string FirstName, string LastName, string MobileNumber);

public sealed class CreateSalonAdminRequest
{
    [Required(ErrorMessage = "نام الزامی است")] public string FirstName { get; set; } = "";
    [Required(ErrorMessage = "نام خانوادگی الزامی است")] public string LastName { get; set; } = "";
    [Required(ErrorMessage = "نام کاربری الزامی است")] public string Username { get; set; } = "";
    [Required(ErrorMessage = "رمز عبور الزامی است")]
    [MinLength(6, ErrorMessage = "رمز عبور حداقل ۶ کاراکتر باشد")]
    public string Password { get; set; } = "";
    [Required(ErrorMessage = "موبایل الزامی است")]
    [RegularExpression(@"^09\d{9}$", ErrorMessage = "شماره موبایل را به شکل 09xxxxxxxxx وارد کنید")]
    public string MobileNumber { get; set; } = "";
    public long SalonId { get; set; }
}

public sealed class UpdateSalonAdminRequest
{
    [Required(ErrorMessage = "نام الزامی است")] public string FirstName { get; set; } = "";
    [Required(ErrorMessage = "نام خانوادگی الزامی است")] public string LastName { get; set; } = "";
    [Required(ErrorMessage = "نام کاربری الزامی است")] public string Username { get; set; } = "";
    [Required(ErrorMessage = "موبایل الزامی است")]
    [RegularExpression(@"^09\d{9}$", ErrorMessage = "شماره موبایل را به شکل 09xxxxxxxxx وارد کنید")]
    public string MobileNumber { get; set; } = "";
}

public sealed record PlatformAdminResponse(long Id, string FirstName, string LastName, string UserName);

public sealed class CreatePlatformAdminRequest
{
    [Required(ErrorMessage = "نام الزامی است")] public string FirstName { get; set; } = "";
    [Required(ErrorMessage = "نام خانوادگی الزامی است")] public string LastName { get; set; } = "";
    [Required(ErrorMessage = "نام کاربری الزامی است")] public string UserName { get; set; } = "";
    [Required(ErrorMessage = "رمز عبور الزامی است")]
    [MinLength(6, ErrorMessage = "رمز عبور حداقل ۶ کاراکتر باشد")]
    public string Password { get; set; } = "";
}

/// <summary>Update بدون id: بک‌اند ادمین جاری را از توکن آپدیت می‌کند.</summary>
public sealed class UpdatePlatformAdminRequest
{
    [Required(ErrorMessage = "نام الزامی است")] public string FirstName { get; set; } = "";
    [Required(ErrorMessage = "نام خانوادگی الزامی است")] public string LastName { get; set; } = "";
    [Required(ErrorMessage = "نام کاربری الزامی است")] public string UserName { get; set; } = "";
}

public sealed record ExceptionLogResponse(
    string Id, string ExceptionType, string Message, string? StackTrace, string? Source,
    string? RequestPath, string? RequestMethod, int StatusCode, DateTimeOffset CreatedAt);

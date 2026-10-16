using System.ComponentModel.DataAnnotations;
using BarberHub.Client.Core.Models;

namespace BarberHub.Client.Features.Barbers.Models;

public sealed record BarberPublicResponse(long Id, string FirstName, string LastName, string? Description, bool IsActive);

public sealed record BarberResponse(long Id, string FirstName, string LastName, string MobileNumber, string? Description, bool IsActive);

public sealed class CreateBarberRequest
{
    [Required(ErrorMessage = "نام الزامی است")] public string FirstName { get; set; } = "";
    [Required(ErrorMessage = "نام خانوادگی الزامی است")] public string LastName { get; set; } = "";
    [Required(ErrorMessage = "موبایل الزامی است")]
    [RegularExpression(@"^09\d{9}$", ErrorMessage = "شماره موبایل را به شکل 09xxxxxxxxx وارد کنید")]
    public string MobileNumber { get; set; } = "";
    [Required(ErrorMessage = "نام کاربری الزامی است")] public string Username { get; set; } = "";
    [Required(ErrorMessage = "رمز عبور الزامی است")]
    [MinLength(6, ErrorMessage = "رمز عبور حداقل ۶ کاراکتر باشد")]
    public string Password { get; set; } = "";
    public string? Description { get; set; }
}

public sealed class UpdateBarberRequest
{
    [Required(ErrorMessage = "نام الزامی است")] public string FirstName { get; set; } = "";
    [Required(ErrorMessage = "نام خانوادگی الزامی است")] public string LastName { get; set; } = "";
    [Required(ErrorMessage = "موبایل الزامی است")]
    [RegularExpression(@"^09\d{9}$", ErrorMessage = "شماره موبایل را به شکل 09xxxxxxxxx وارد کنید")]
    public string MobileNumber { get; set; } = "";
    /// <summary>BarberResponse نام کاربری را برنمی‌گرداند؛ کاربر باید آن را وارد کند.</summary>
    [Required(ErrorMessage = "نام کاربری الزامی است")] public string Username { get; set; } = "";
    public string? Description { get; set; }

    public static UpdateBarberRequest From(BarberResponse b) => new()
    { FirstName = b.FirstName, LastName = b.LastName, MobileNumber = b.MobileNumber, Description = b.Description };
}

// BarberServiceController: قیمت و مدت هر خدمت برای هر آرایشگر
public sealed record BarberServiceResponse(long Id, long BarberId, long ServiceId, decimal PriceValue, Currency PriceCurrency, TimeSpan Duration);

public sealed record CreateBarberServiceRequest(long BarberId, long ServiceId, decimal PriceValue, Currency PriceCurrency, TimeSpan Duration);

public sealed record UpdateBarberServiceRequest(decimal PriceValue, Currency PriceCurrency, TimeSpan Duration);

/// <summary>مدل فرم: مدت به دقیقه، بعد به TimeSpan تبدیل می‌شود.</summary>
public sealed class BarberServiceForm
{
    [Range(1, long.MaxValue, ErrorMessage = "یک خدمت انتخاب کنید")] public long ServiceId { get; set; }
    [Range(1, double.MaxValue, ErrorMessage = "قیمت را وارد کنید")] public decimal PriceValue { get; set; }
    [Range(5, 600, ErrorMessage = "مدت بین ۵ تا ۶۰۰ دقیقه")] public int DurationMinutes { get; set; } = 30;
    public TimeSpan Duration => TimeSpan.FromMinutes(DurationMinutes);
}

using System.ComponentModel.DataAnnotations;
using BarberHub.Client.Core.Models;

namespace BarberHub.Client.Features.Salons.Models;

public sealed record SalonResponse(
    long Id, string Name, string Address, string City, string PhoneNumber,
    decimal DepositAmountValue, Currency DepositAmountCurrency, string? Description, bool IsActive);

public sealed class CreateSalonRequest
{
    [Required(ErrorMessage = "نام سالن الزامی است")] public string Name { get; set; } = "";
    [Required(ErrorMessage = "آدرس الزامی است")] public string Address { get; set; } = "";
    [Required(ErrorMessage = "شهر الزامی است")] public string City { get; set; } = "";
    [Required(ErrorMessage = "تلفن الزامی است")] public string PhoneNumber { get; set; } = "";
    [Range(0, double.MaxValue, ErrorMessage = "مبلغ معتبر نیست")] public decimal DepositAmountValue { get; set; }
    public Currency DepositAmountCurrency { get; set; } = Currency.Irr;
    public string? Description { get; set; }
}

public sealed class UpdateSalonRequest
{
    [Required(ErrorMessage = "نام سالن الزامی است")] public string Name { get; set; } = "";
    [Required(ErrorMessage = "آدرس الزامی است")] public string Address { get; set; } = "";
    [Required(ErrorMessage = "شهر الزامی است")] public string City { get; set; } = "";
    [Required(ErrorMessage = "تلفن الزامی است")] public string PhoneNumber { get; set; } = "";
    public string? Description { get; set; }

    public static UpdateSalonRequest From(SalonResponse s) => new()
    { Name = s.Name, Address = s.Address, City = s.City, PhoneNumber = s.PhoneNumber, Description = s.Description };
}

public sealed class UpdateSalonDepositAmountRequest
{
    [Range(0, double.MaxValue, ErrorMessage = "مبلغ معتبر نیست")] public decimal DepositAmountValue { get; set; }
    public Currency DepositAmountCurrency { get; set; } = Currency.Irr;
}

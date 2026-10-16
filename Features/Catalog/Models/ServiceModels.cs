using System.ComponentModel.DataAnnotations;

namespace BarberHub.Client.Features.Catalog.Models;

// ServiceController: کاتالوگ خدمات سالن (اصلاح، رنگ، ...)
public sealed record ServiceResponse(long Id, string Name, string? Description);

public sealed class ServiceForm
{
    [Required(ErrorMessage = "نام خدمت الزامی است")] public string Name { get; set; } = "";
    public string? Description { get; set; }
}

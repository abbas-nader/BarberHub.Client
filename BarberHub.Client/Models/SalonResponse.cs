namespace BarberHub.Client.Models;

public record SalonResponse(
    long Id,
    string Name,
    string Address,
    string City,
    string PhoneNumber,
    decimal DepositAmountValue,
    byte DepositAmountCurrency,
    string? Description,
    bool IsActive
);
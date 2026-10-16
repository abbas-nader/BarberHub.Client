namespace BarberHub.Client.Core.Models;

// مقادیر عددی دقیقا مطابق BarberHub.Domain.Enums (سریال‌سازی پیش‌فرض: عدد)
public enum UserRole : byte
{
    PlatformAdmin = 1,
    SalonAdmin = 2,
    Barber = 3,
    EndUser = 4
}

public enum Currency : byte
{
    Irr = 1
}

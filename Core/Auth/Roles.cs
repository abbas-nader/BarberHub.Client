namespace BarberHub.Client.Core.Auth;

/// <summary>نام نقش‌ها همان nameof(UserRole) در بک‌اند است و در claim نقش JWT قرار می‌گیرد.</summary>
public static class Roles
{
    public const string PlatformAdmin = nameof(Core.Models.UserRole.PlatformAdmin);
    public const string SalonAdmin = nameof(Core.Models.UserRole.SalonAdmin);
    public const string Barber = nameof(Core.Models.UserRole.Barber);
    public const string EndUser = nameof(Core.Models.UserRole.EndUser);
}

public static class AppClaims
{
    public const string UserId = "sub";
    public const string Role = "role";
    public const string SalonId = "salonid";   // JwtCustomClaimNames.SalonId
}

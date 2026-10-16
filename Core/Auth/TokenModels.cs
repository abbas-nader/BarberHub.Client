namespace BarberHub.Client.Core.Auth;

// مطابق Contracts/Auth/TokenResponse.cs
public sealed record TokenResponse(
    string AccessToken,
    DateTimeOffset AccessTokenExpireAt,
    string RefreshToken,
    DateTimeOffset RefreshTokenExpireAt);

public sealed record RefreshRequest(string RefreshToken);

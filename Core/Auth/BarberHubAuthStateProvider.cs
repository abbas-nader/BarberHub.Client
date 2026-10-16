using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace BarberHub.Client.Core.Auth;

/// <summary>
/// وضعیت لاگین را از JWT ذخیره‌شده می‌سازد. با هر تغییر TokenStore (login/refresh/logout) UI خودکار آپدیت می‌شود.
/// </summary>
public sealed class BarberHubAuthStateProvider : AuthenticationStateProvider, IDisposable
{
    private static readonly AuthenticationState Anonymous = new(new ClaimsPrincipal(new ClaimsIdentity()));
    private readonly TokenStore _store;
    private readonly TokenRefresher _refresher;

    public BarberHubAuthStateProvider(TokenStore store, TokenRefresher refresher)
    {
        _store = store; _refresher = refresher;
        _store.Changed += OnTokensChanged;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _refresher.GetValidAccessTokenAsync();
        if (token is null) return Anonymous;

        try
        {
            var identity = new ClaimsIdentity(JwtParser.ParseClaims(token), "jwt", AppClaims.UserId, AppClaims.Role);
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
        catch
        {
            await _store.ClearAsync();
            return Anonymous;
        }
    }

    private void OnTokensChanged() => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

    public void Dispose() => _store.Changed -= OnTokensChanged;
}

public static class ClaimsPrincipalExtensions
{
    public static long? UserId(this ClaimsPrincipal u)
        => long.TryParse(u.FindFirst(AppClaims.UserId)?.Value, out var id) ? id : null;

    public static long? SalonId(this ClaimsPrincipal u)
        => long.TryParse(u.FindFirst(AppClaims.SalonId)?.Value, out var id) ? id : null;

    public static string? RoleName(this ClaimsPrincipal u) => u.FindFirst(AppClaims.Role)?.Value;
}

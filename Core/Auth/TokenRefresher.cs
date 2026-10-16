using System.Net.Http.Json;
using BarberHub.Client.Core.Api;

namespace BarberHub.Client.Core.Auth;

/// <summary>
/// تمدید توکن با POST auth/refresh. چون بک‌اند refresh token را می‌چرخاند (rotation و reuse detection)،
/// با SemaphoreSlim مطمئن می‌شویم هم‌زمان فقط یک refresh انجام شود.
/// </summary>
public sealed class TokenRefresher(IHttpClientFactory factory, TokenStore store)
{
    private static readonly TimeSpan Skew = TimeSpan.FromSeconds(30);
    private readonly SemaphoreSlim _gate = new(1, 1);

    /// <summary>توکن معتبر را برمی‌گرداند؛ اگر نزدیک انقضا بود تمدید می‌کند؛ اگر نشد null.</summary>
    public async Task<string?> GetValidAccessTokenAsync(bool forceRefresh = false)
    {
        var tokens = await store.GetAsync();
        if (tokens is null) return null;
        if (!forceRefresh && tokens.AccessTokenExpireAt - Skew > DateTimeOffset.UtcNow)
            return tokens.AccessToken;

        await _gate.WaitAsync();
        try
        {
            // شاید درخواست دیگری در همین فاصله refresh کرده باشد
            var current = await store.GetAsync();
            if (current is null) return null;
            if (current.AccessToken != tokens.AccessToken && current.AccessTokenExpireAt - Skew > DateTimeOffset.UtcNow)
                return current.AccessToken;

            if (current.RefreshTokenExpireAt <= DateTimeOffset.UtcNow)
            {
                await store.ClearAsync();
                return null;
            }

            var http = factory.CreateClient(ApiOptions.AnonymousClient);
            using var res = await http.PostAsJsonAsync(ApiEndpoints.Auth.Refresh, new RefreshRequest(current.RefreshToken), ApiClient.Json);
            if (!res.IsSuccessStatusCode)
            {
                if ((int)res.StatusCode is 401 or 409 or 400) await store.ClearAsync();
                return null;
            }

            var env = await res.Content.ReadFromJsonAsync<ApiEnvelope<TokenResponse>>(ApiClient.Json);
            if (env?.Data is null) { await store.ClearAsync(); return null; }

            await store.SetAsync(env.Data);
            return env.Data.AccessToken;
        }
        catch
        {
            return null; // خطای شبکه: توکن‌ها را پاک نکن، شاید موقتی باشد
        }
        finally
        {
            _gate.Release();
        }
    }
}

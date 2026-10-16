using System.Net.Http.Json;
using BarberHub.Client.Core.Api;
using BarberHub.Client.Core.Models;
using BarberHub.Client.Features.Auth.Models;

namespace BarberHub.Client.Core.Auth;

/// <summary>login / register / logout. از کلاینت anonymous استفاده می‌کند تا وارد حلقه‌ی refresh نشود.</summary>
public sealed class AuthService(IHttpClientFactory factory, TokenStore store)
{
    private HttpClient Http => factory.CreateClient(ApiOptions.AnonymousClient);

    public async Task<ApiResponse> LoginAsync(LoginRequest request)
    {
        var res = await PostAsync<TokenResponse>(ApiEndpoints.Auth.Login, request);
        if (res.Success && !string.IsNullOrEmpty(res.Data?.AccessToken)) await store.SetAsync(res.Data);
        return res;
    }

    /// <summary>ثبت‌نام فقط برای مشتری (EndUser) است و بک‌اند مستقیما توکن برمی‌گرداند.</summary>
    public async Task<ApiResponse> RegisterAsync(RegisterRequest request)
    {
        var res = await PostAsync<TokenResponse>(ApiEndpoints.Auth.Register, request);
        if (res.Success && !string.IsNullOrEmpty(res.Data?.AccessToken)) await store.SetAsync(res.Data);
        return res;
    }

    public async Task<bool> IsLoggedInAsync() => await store.GetAsync() is not null;

    public async Task LogoutAsync()
    {
        var tokens = await store.GetAsync();
        if (tokens is not null)
        {
            // revoke به هر دلیل شکست بخورد، لاگ‌اوت محلی انجام می‌شود
            try { await Http.PostAsJsonAsync(ApiEndpoints.Auth.Revoke, new RefreshRequest(tokens.RefreshToken), ApiClient.Json); }
            catch { /* ignore */ }
        }
        await store.ClearAsync();
    }

    private async Task<ApiResponse<T>> PostAsync<T>(string url, object body)
    {
        try
        {
            using var res = await Http.PostAsync(url, JsonContent.Create(body, body.GetType(), options: ApiClient.Json));
            var text = await res.Content.ReadAsStringAsync();
            if (!res.IsSuccessStatusCode) return ApiResponse<T>.From(ApiErrorParser.Parse((int)res.StatusCode, text));
            var env = System.Text.Json.JsonSerializer.Deserialize<ApiEnvelope<T>>(text, ApiClient.Json);
            return ApiResponse<T>.Ok(env is null ? default : env.Data);
        }
        catch
        {
            return ApiResponse<T>.Fail(0, ApiErrorParser.DefaultMessage(0));
        }
    }
}

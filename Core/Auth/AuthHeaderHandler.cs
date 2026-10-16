using System.Net;
using System.Net.Http.Headers;

namespace BarberHub.Client.Core.Auth;

/// <summary>
/// 1) قبل از ارسال: Authorization: Bearer {token} (با تمدید پیش‌دستانه)
/// 2) اگر 401 گرفت: یک بار refresh و تکرار درخواست
/// </summary>
public sealed class AuthHeaderHandler(TokenRefresher refresher) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        var token = await refresher.GetValidAccessTokenAsync();
        if (token is not null)
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await base.SendAsync(request, ct);

        if (response.StatusCode != HttpStatusCode.Unauthorized || token is null)
            return response;

        var fresh = await refresher.GetValidAccessTokenAsync(forceRefresh: true);
        if (fresh is null) return response;

        var retry = await CloneAsync(request);
        retry.Headers.Authorization = new AuthenticationHeaderValue("Bearer", fresh);
        response.Dispose();
        return await base.SendAsync(retry, ct);
    }

    private static async Task<HttpRequestMessage> CloneAsync(HttpRequestMessage req)
    {
        var clone = new HttpRequestMessage(req.Method, req.RequestUri);
        if (req.Content is not null)
        {
            var bytes = await req.Content.ReadAsByteArrayAsync();
            clone.Content = new ByteArrayContent(bytes);
            foreach (var h in req.Content.Headers) clone.Content.Headers.TryAddWithoutValidation(h.Key, h.Value);
        }
        foreach (var h in req.Headers) clone.Headers.TryAddWithoutValidation(h.Key, h.Value);
        return clone;
    }
}

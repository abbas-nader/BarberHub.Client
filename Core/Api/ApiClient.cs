using System.Net.Http.Json;
using System.Text.Json;

namespace BarberHub.Client.Core.Api;

/// <summary>
/// تنها نقطه‌ی تماس HTTP برای سرویس‌های Feature.
/// envelope را باز می‌کند، خطاها را به ApiResponse تبدیل می‌کند و exception را بیرون نمی‌دهد.
/// هدر Authorization توسط AuthHeaderHandler اضافه می‌شود.
/// </summary>
public sealed class ApiClient(HttpClient http)
{
    public static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web);

    public Task<ApiResponse<T>> GetAsync<T>(string url, CancellationToken ct = default)
        => SendAsync<T>(() => new HttpRequestMessage(HttpMethod.Get, url), ct);

    public Task<ApiResponse<T>> PostAsync<T>(string url, object? body, CancellationToken ct = default)
        => SendAsync<T>(() => WithJson(HttpMethod.Post, url, body), ct);

    public Task<ApiResponse<T>> PutAsync<T>(string url, object? body, CancellationToken ct = default)
        => SendAsync<T>(() => WithJson(HttpMethod.Put, url, body), ct);

    public Task<ApiResponse<T>> PatchAsync<T>(string url, object? body = null, CancellationToken ct = default)
        => SendAsync<T>(() => WithJson(HttpMethod.Patch, url, body), ct);

    public async Task<ApiResponse> PostAsync(string url, object? body, CancellationToken ct = default)
        => await SendAsync<JsonElement>(() => WithJson(HttpMethod.Post, url, body), ct);

    /// <summary>برای multipart (آپلود گالری). factory چون در retry باید محتوا دوباره ساخته شود.</summary>
    public Task<ApiResponse<T>> PostMultipartAsync<T>(string url, Func<MultipartFormDataContent> content, CancellationToken ct = default)
        => SendAsync<T>(() => new HttpRequestMessage(HttpMethod.Post, url) { Content = content() }, ct);

    private static HttpRequestMessage WithJson(HttpMethod method, string url, object? body)
    {
        var req = new HttpRequestMessage(method, url);
        if (body is not null) req.Content = JsonContent.Create(body, body.GetType(), options: Json);
        return req;
    }

    private async Task<ApiResponse<T>> SendAsync<T>(Func<HttpRequestMessage> factory, CancellationToken ct)
    {
        HttpResponseMessage response;
        try
        {
            response = await http.SendAsync(factory(), ct);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception)
        {
            // CORS، قطعی شبکه، آدرس اشتباه API
            return ApiResponse<T>.Fail(0, ApiErrorParser.DefaultMessage(0));
        }

        using (response)
        {
            var status = (int)response.StatusCode;
            var body = response.Content is null ? null : await response.Content.ReadAsStringAsync(ct);

            if (!response.IsSuccessStatusCode)
                return ApiResponse<T>.From(ApiErrorParser.Parse(status, body));

            if (status == 204 || string.IsNullOrWhiteSpace(body))
                return ApiResponse<T>.Ok(default, status);

            try
            {
                var envelope = JsonSerializer.Deserialize<ApiEnvelope<T>>(body, Json);
                return envelope is { Success: true }
                    ? ApiResponse<T>.Ok(envelope.Data, status)
                    : ApiResponse<T>.From(ApiErrorParser.Parse(envelope?.Status ?? status, body));
            }
            catch (JsonException)
            {
                return ApiResponse<T>.Fail(status, "پاسخ سرور قابل خواندن نبود.");
            }
        }
    }
}

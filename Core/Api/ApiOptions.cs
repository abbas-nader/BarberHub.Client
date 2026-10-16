namespace BarberHub.Client.Core.Api;

public sealed class ApiOptions
{
    public const string AnonymousClient = "BarberHub.Anonymous";

    /// <summary>آدرس هاست API، مثلا https://localhost:5001/</summary>
    public string BaseUrl { get; set; } = "https://localhost:5001/";

    /// <summary>نسخه‌ی API (بک‌اند: api/v{version}/)</summary>
    public string Version { get; set; } = "v1";

    public Uri HostRoot => new(BaseUrl.EndsWith('/') ? BaseUrl : BaseUrl + "/");

    /// <summary>ریشه‌ی همه‌ی endpointها: {BaseUrl}/api/v1/</summary>
    public Uri ApiRoot => new(HostRoot, $"api/{Version}/");

    /// <summary>برای URLهای نسبی فایل‌ها (مثل FileResponse.Url)</summary>
    public string ToAbsoluteUrl(string url)
        => Uri.TryCreate(url, UriKind.Absolute, out var abs) ? abs.ToString() : new Uri(HostRoot, url.TrimStart('/')).ToString();
}

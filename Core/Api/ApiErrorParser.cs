using System.Text.Json;

namespace BarberHub.Client.Core.Api;

/// <summary>
/// بک‌اند سه شکل خطا دارد:
/// 1) ApiResult با error=null (401/403/404/429)
/// 2) ApiResult با error = ValidationProblemDetails (400 از ValidationFilter)
/// 3) problem+json از ExceptionHandlingMiddleware: { status, title, detail }
/// </summary>
public static class ApiErrorParser
{
    public static ApiResponse Parse(int status, string? body)
    {
        var fields = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);
        string? detail = null;

        if (!string.IsNullOrWhiteSpace(body))
        {
            try
            {
                using var doc = JsonDocument.Parse(body);
                var root = doc.RootElement;
                var source = root;

                // envelope؟ برو داخل error
                if (TryGet(root, "error", out var err) && err.ValueKind == JsonValueKind.Object)
                    source = err;
                else if (TryGet(root, "error", out var errStr) && errStr.ValueKind == JsonValueKind.String)
                    detail = errStr.GetString();

                if (TryGet(source, "errors", out var errors) && errors.ValueKind == JsonValueKind.Object)
                {
                    foreach (var p in errors.EnumerateObject())
                        fields[p.Name] = p.Value.ValueKind == JsonValueKind.Array
                            ? p.Value.EnumerateArray().Select(x => x.GetString() ?? "").ToArray()
                            : [p.Value.ToString()];
                }

                if (detail is null && TryGet(source, "detail", out var d) && d.ValueKind == JsonValueKind.String)
                    detail = d.GetString();
            }
            catch (JsonException)
            {
                // بدنه JSON نیست؛ پیام پیش‌فرض
            }
        }

        var message = fields.Count > 0
            ? "لطفا خطاهای فرم را بررسی کنید."
            : detail is { Length: > 0 } && status != 500 ? detail : DefaultMessage(status);

        return ApiResponse.Fail(status, message, fields);
    }

    public static string DefaultMessage(int status) => status switch
    {
        0 => "ارتباط با سرور برقرار نشد. اتصال اینترنت یا آدرس API را بررسی کنید.",
        400 => "اطلاعات ارسال‌شده معتبر نیست.",
        401 => "نشست شما منقضی شده یا نام کاربری/رمز عبور اشتباه است.",
        403 => "اجازه‌ی انجام این کار را ندارید.",
        404 => "مورد درخواستی پیدا نشد.",
        409 => "این عملیات با وضعیت فعلی داده‌ها تداخل دارد.",
        429 => "تعداد درخواست‌ها زیاد است. چند دقیقه بعد دوباره تلاش کنید.",
        502 => "سرویس پیامک در دسترس نیست. بعدا تلاش کنید.",
        _ => "خطای غیرمنتظره‌ای در سرور رخ داد."
    };

    private static bool TryGet(JsonElement e, string name, out JsonElement value)
    {
        value = default;
        if (e.ValueKind != JsonValueKind.Object) return false;
        foreach (var p in e.EnumerateObject())
            if (string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase)) { value = p.Value; return true; }
        return false;
    }
}

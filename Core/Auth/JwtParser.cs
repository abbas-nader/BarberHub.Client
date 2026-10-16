using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace BarberHub.Client.Core.Auth;

/// <summary>
/// فقط payload را برای ساخت ClaimsPrincipal می‌خواند؛ اعتبارسنجی امضا کار سرور است.
/// نقش ممکن است با کلید "role" یا URI کامل ClaimTypes.Role بیاید؛ هر دو را به "role" نرمال می‌کنیم.
/// </summary>
public static class JwtParser
{
    public static IEnumerable<Claim> ParseClaims(string jwt)
    {
        var parts = jwt.Split('.');
        if (parts.Length < 2) yield break;

        var json = Encoding.UTF8.GetString(Base64UrlDecode(parts[1]));
        using var doc = JsonDocument.Parse(json);

        foreach (var p in doc.RootElement.EnumerateObject())
        {
            var type = p.Name == ClaimTypes.Role ? AppClaims.Role : p.Name;
            if (p.Value.ValueKind == JsonValueKind.Array)
            {
                foreach (var v in p.Value.EnumerateArray())
                    yield return new Claim(type, v.ToString());
            }
            else
            {
                yield return new Claim(type, p.Value.ToString());
            }
        }
    }

    private static byte[] Base64UrlDecode(string input)
    {
        var s = input.Replace('-', '+').Replace('_', '/');
        s = (s.Length % 4) switch { 2 => s + "==", 3 => s + "=", _ => s };
        return Convert.FromBase64String(s);
    }
}

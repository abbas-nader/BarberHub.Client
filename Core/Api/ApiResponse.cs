using System.Text.Json;

namespace BarberHub.Client.Core.Api;

/// <summary>شکل envelope بک‌اند: { success, status, error, data }</summary>
public sealed class ApiEnvelope<T>
{
    public bool Success { get; set; }
    public int Status { get; set; }
    public JsonElement? Error { get; set; }
    public T? Data { get; set; }
}

/// <summary>نتیجه‌ی یکدست برای UI. هیچ‌وقت exception به کامپوننت نمی‌رسد.</summary>
public class ApiResponse
{
    public bool Success { get; init; }
    public int Status { get; init; }
    public string? ErrorMessage { get; init; }
    public IReadOnlyDictionary<string, string[]> FieldErrors { get; init; } = new Dictionary<string, string[]>();

    public bool IsUnauthorized => Status == 401;
    public bool IsForbidden => Status == 403;
    public bool IsNotFound => Status == 404;

    public static ApiResponse Ok(int status = 200) => new() { Success = true, Status = status };
    public static ApiResponse Fail(int status, string message, IReadOnlyDictionary<string, string[]>? fields = null)
        => new() { Success = false, Status = status, ErrorMessage = message, FieldErrors = fields ?? new Dictionary<string, string[]>() };
}

public sealed class ApiResponse<T> : ApiResponse
{
    public T? Data { get; init; }

    public static ApiResponse<T> Ok(T? data, int status = 200) => new() { Success = true, Status = status, Data = data };
    public static new ApiResponse<T> Fail(int status, string message, IReadOnlyDictionary<string, string[]>? fields = null)
        => new() { Success = false, Status = status, ErrorMessage = message, FieldErrors = fields ?? new Dictionary<string, string[]>() };
    public static ApiResponse<T> From(ApiResponse r)
        => new() { Success = r.Success, Status = r.Status, ErrorMessage = r.ErrorMessage, FieldErrors = r.FieldErrors };
}

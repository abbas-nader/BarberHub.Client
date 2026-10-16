using System.Text.Json;
using Microsoft.JSInterop;

namespace BarberHub.Client.Core.Auth;

/// <summary>
/// نگهداری توکن‌ها در localStorage + کش در حافظه.
/// Singleton است تا AuthHeaderHandler و AuthStateProvider یک نمونه‌ی مشترک ببینند.
/// </summary>
public sealed class TokenStore(IJSRuntime js)
{
    private const string Key = "barberhub.tokens";
    private TokenResponse? _cache;
    private bool _loaded;

    /// <summary>هر بار که توکن عوض یا پاک شد (login/refresh/logout).</summary>
    public event Action? Changed;

    public async ValueTask<TokenResponse?> GetAsync()
    {
        if (_loaded) return _cache;
        try
        {
            var raw = await js.InvokeAsync<string?>("localStorage.getItem", Key);
            _cache = string.IsNullOrWhiteSpace(raw) ? null : JsonSerializer.Deserialize<TokenResponse>(raw);
        }
        catch
        {
            _cache = null; // داده‌ی خراب در storage
        }
        _loaded = true;
        return _cache;
    }

    public async ValueTask SetAsync(TokenResponse tokens)
    {
        _cache = tokens; _loaded = true;
        await js.InvokeVoidAsync("localStorage.setItem", Key, JsonSerializer.Serialize(tokens));
        Changed?.Invoke();
    }

    public async ValueTask ClearAsync()
    {
        var had = _cache is not null;
        _cache = null; _loaded = true;
        await js.InvokeVoidAsync("localStorage.removeItem", Key);
        if (had) Changed?.Invoke();
    }
}

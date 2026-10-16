using BarberHub.Client.Core.Api;
using BarberHub.Client.Core.Models;
using BarberHub.Client.Features.Salons.Models;

namespace BarberHub.Client.Features.Salons.Services;

/// <summary>SalonController</summary>
public sealed class SalonApi(ApiClient api)
{
    /// <summary>عمومی (AllowAnonymous)</summary>
    public Task<ApiResponse<List<SalonResponse>>> GetAllAsync() => api.GetAsync<List<SalonResponse>>(ApiEndpoints.Salon.GetAll);

    /// <summary>نیاز به لاگین (هر نقش)</summary>
    public Task<ApiResponse<PaginatedResponse<SalonResponse>>> GetPaginatedAsync(int page, int size)
        => api.GetAsync<PaginatedResponse<SalonResponse>>(ApiEndpoints.Salon.Paginated(page, size));

    /// <summary>نیاز به لاگین (هر نقش)</summary>
    public Task<ApiResponse<SalonResponse>> GetByIdAsync(long id) => api.GetAsync<SalonResponse>(ApiEndpoints.Salon.GetById(id));

    /// <summary>
    /// برای صفحه‌ی عمومی: GetById نیاز به لاگین دارد، پس برای مهمان از لیست عمومی پیدا می‌کنیم.
    /// </summary>
    public async Task<ApiResponse<SalonResponse>> GetPublicAsync(long id, bool isAuthenticated)
    {
        if (isAuthenticated)
        {
            var direct = await GetByIdAsync(id);
            if (direct.Success || direct.IsNotFound) return direct;
        }
        var all = await GetAllAsync();
        if (!all.Success) return ApiResponse<SalonResponse>.From(all);
        var found = all.Data?.FirstOrDefault(s => s.Id == id);
        return found is null ? ApiResponse<SalonResponse>.Fail(404, "سالن پیدا نشد.") : ApiResponse<SalonResponse>.Ok(found);
    }

    // PlatformAdmin
    public Task<ApiResponse<SalonResponse>> CreateAsync(CreateSalonRequest r) => api.PostAsync<SalonResponse>(ApiEndpoints.Salon.Create, r);
    public Task<ApiResponse<SalonResponse>> DeleteAsync(long id) => api.PatchAsync<SalonResponse>(ApiEndpoints.Salon.Delete(id));
    public Task<ApiResponse<SalonResponse>> ActivateAsync(long id) => api.PatchAsync<SalonResponse>(ApiEndpoints.Salon.Activate(id));
    public Task<ApiResponse<SalonResponse>> DeactivateAsync(long id) => api.PatchAsync<SalonResponse>(ApiEndpoints.Salon.Deactivate(id));

    // SalonAdmin (سالن از claim salonid توکن تشخیص داده می‌شود)
    public Task<ApiResponse<SalonResponse>> UpdateMySalonAsync(UpdateSalonRequest r) => api.PutAsync<SalonResponse>(ApiEndpoints.Salon.Update, r);
    public Task<ApiResponse<SalonResponse>> UpdateDepositAsync(UpdateSalonDepositAmountRequest r) => api.PatchAsync<SalonResponse>(ApiEndpoints.Salon.UpdateDepositAmount, r);
}

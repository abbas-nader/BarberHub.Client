using BarberHub.Client.Core.Api;
using BarberHub.Client.Features.PlatformAdmin.Models;

namespace BarberHub.Client.Features.PlatformAdmin.Services;

/// <summary>SalonAdminController. Update/Delete برای SalonAdmin (خودش) و PlatformAdmin مجاز است.</summary>
public sealed class SalonAdminApi(ApiClient api)
{
    public Task<ApiResponse<List<SalonAdminResponse>>> GetBySalonAsync(long salonId)
        => api.GetAsync<List<SalonAdminResponse>>(ApiEndpoints.SalonAdmin.GetAllBySalon(salonId));
    public Task<ApiResponse<SalonAdminResponse>> GetByIdAsync(long id) => api.GetAsync<SalonAdminResponse>(ApiEndpoints.SalonAdmin.GetById(id));
    public Task<ApiResponse<SalonAdminResponse>> CreateAsync(CreateSalonAdminRequest r) => api.PostAsync<SalonAdminResponse>(ApiEndpoints.SalonAdmin.Create, r);
    public Task<ApiResponse<SalonAdminResponse>> UpdateAsync(long id, UpdateSalonAdminRequest r) => api.PutAsync<SalonAdminResponse>(ApiEndpoints.SalonAdmin.Update(id), r);
    public Task<ApiResponse<SalonAdminResponse>> DeleteAsync(long id) => api.PatchAsync<SalonAdminResponse>(ApiEndpoints.SalonAdmin.Delete(id));
}

/// <summary>PlatformAdminController (همه‌ی متدها فقط PlatformAdmin)</summary>
public sealed class PlatformAdminApi(ApiClient api)
{
    public Task<ApiResponse<List<PlatformAdminResponse>>> GetAllAsync() => api.GetAsync<List<PlatformAdminResponse>>(ApiEndpoints.PlatformAdmin.GetAll);
    public Task<ApiResponse<PlatformAdminResponse>> GetByIdAsync(long id) => api.GetAsync<PlatformAdminResponse>(ApiEndpoints.PlatformAdmin.GetById(id));
    public Task<ApiResponse<PlatformAdminResponse>> CreateAsync(CreatePlatformAdminRequest r) => api.PostAsync<PlatformAdminResponse>(ApiEndpoints.PlatformAdmin.Create, r);
    public Task<ApiResponse<PlatformAdminResponse>> UpdateMeAsync(UpdatePlatformAdminRequest r) => api.PutAsync<PlatformAdminResponse>(ApiEndpoints.PlatformAdmin.Update, r);
    public Task<ApiResponse<PlatformAdminResponse>> DeleteAsync(long id) => api.PatchAsync<PlatformAdminResponse>(ApiEndpoints.PlatformAdmin.Delete(id));
}

/// <summary>ExceptionLogController (فقط PlatformAdmin)</summary>
public sealed class ExceptionLogApi(ApiClient api)
{
    public Task<ApiResponse<List<ExceptionLogResponse>>> GetAllAsync() => api.GetAsync<List<ExceptionLogResponse>>(ApiEndpoints.ExceptionLog.GetAll);
    public Task<ApiResponse<List<ExceptionLogResponse>>> GetRecentAsync(int count) => api.GetAsync<List<ExceptionLogResponse>>(ApiEndpoints.ExceptionLog.Recent(count));
    public Task<ApiResponse<ExceptionLogResponse>> GetByIdAsync(string id) => api.GetAsync<ExceptionLogResponse>(ApiEndpoints.ExceptionLog.GetById(id));
}

using BarberHub.Client.Core.Api;
using BarberHub.Client.Features.Catalog.Models;

namespace BarberHub.Client.Features.Catalog.Services;

/// <summary>ServiceController. ایجاد خدمت salonId نمی‌گیرد؛ از توکن SalonAdmin خوانده می‌شود.</summary>
public sealed class ServiceCatalogApi(ApiClient api)
{
    public Task<ApiResponse<List<ServiceResponse>>> GetBySalonAsync(long salonId)
        => api.GetAsync<List<ServiceResponse>>(ApiEndpoints.Service.GetAllBySalon(salonId));
    public Task<ApiResponse<ServiceResponse>> GetByIdAsync(long id) => api.GetAsync<ServiceResponse>(ApiEndpoints.Service.GetById(id));
    public Task<ApiResponse<ServiceResponse>> CreateAsync(ServiceForm f) => api.PostAsync<ServiceResponse>(ApiEndpoints.Service.Create, f);
    public Task<ApiResponse<ServiceResponse>> UpdateAsync(long id, ServiceForm f) => api.PutAsync<ServiceResponse>(ApiEndpoints.Service.Update(id), f);
    public Task<ApiResponse<ServiceResponse>> DeleteAsync(long id) => api.PatchAsync<ServiceResponse>(ApiEndpoints.Service.Delete(id));
}

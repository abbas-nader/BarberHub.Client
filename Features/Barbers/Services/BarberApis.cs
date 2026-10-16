using BarberHub.Client.Core.Api;
using BarberHub.Client.Features.Barbers.Models;

namespace BarberHub.Client.Features.Barbers.Services;

/// <summary>BarberController</summary>
public sealed class BarberApi(ApiClient api)
{
    public Task<ApiResponse<List<BarberPublicResponse>>> GetPublicBySalonAsync(long salonId)
        => api.GetAsync<List<BarberPublicResponse>>(ApiEndpoints.Barber.GetAllPublicBySalon(salonId));

    /// <summary>عمومی؛ شامل موبایل هم هست</summary>
    public Task<ApiResponse<BarberResponse>> GetByIdAsync(long id) => api.GetAsync<BarberResponse>(ApiEndpoints.Barber.GetById(id));

    // SalonAdmin
    public Task<ApiResponse<List<BarberResponse>>> GetBySalonAsync(long salonId)
        => api.GetAsync<List<BarberResponse>>(ApiEndpoints.Barber.GetAllBySalon(salonId));
    public Task<ApiResponse<BarberResponse>> CreateAsync(CreateBarberRequest r) => api.PostAsync<BarberResponse>(ApiEndpoints.Barber.Create, r);
    public Task<ApiResponse<BarberResponse>> UpdateAsync(long id, UpdateBarberRequest r) => api.PutAsync<BarberResponse>(ApiEndpoints.Barber.Update(id), r);
    public Task<ApiResponse<BarberResponse>> DeleteAsync(long id) => api.PatchAsync<BarberResponse>(ApiEndpoints.Barber.Delete(id));
    public Task<ApiResponse<BarberResponse>> ActivateAsync(long id) => api.PatchAsync<BarberResponse>(ApiEndpoints.Barber.Activate(id));
    public Task<ApiResponse<BarberResponse>> DeactivateAsync(long id) => api.PatchAsync<BarberResponse>(ApiEndpoints.Barber.Deactivate(id));
}

/// <summary>BarberServiceController</summary>
public sealed class BarberServiceApi(ApiClient api)
{
    public Task<ApiResponse<List<BarberServiceResponse>>> GetByBarberAsync(long barberId)
        => api.GetAsync<List<BarberServiceResponse>>(ApiEndpoints.BarberService.GetAllByBarber(barberId));
    public Task<ApiResponse<BarberServiceResponse>> GetByIdAsync(long id) => api.GetAsync<BarberServiceResponse>(ApiEndpoints.BarberService.GetById(id));
    public Task<ApiResponse<BarberServiceResponse>> CreateAsync(CreateBarberServiceRequest r) => api.PostAsync<BarberServiceResponse>(ApiEndpoints.BarberService.Create, r);
    public Task<ApiResponse<BarberServiceResponse>> UpdateAsync(long id, UpdateBarberServiceRequest r) => api.PutAsync<BarberServiceResponse>(ApiEndpoints.BarberService.Update(id), r);
    public Task<ApiResponse<BarberServiceResponse>> DeleteAsync(long id) => api.PatchAsync<BarberServiceResponse>(ApiEndpoints.BarberService.Delete(id));
}

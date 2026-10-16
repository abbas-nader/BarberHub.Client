using BarberHub.Client.Core.Api;
using BarberHub.Client.Features.WorkSchedules.Models;

namespace BarberHub.Client.Features.WorkSchedules.Services;

/// <summary>WorkScheduleController</summary>
public sealed class WorkScheduleApi(ApiClient api)
{
    public Task<ApiResponse<List<WorkScheduleResponse>>> GetByBarberAsync(long barberId)
        => api.GetAsync<List<WorkScheduleResponse>>(ApiEndpoints.WorkSchedule.GetAllByBarber(barberId));
    public Task<ApiResponse<WorkScheduleResponse>> GetByIdAsync(long id) => api.GetAsync<WorkScheduleResponse>(ApiEndpoints.WorkSchedule.GetById(id));
    public Task<ApiResponse<WorkScheduleResponse>> CreateAsync(CreateWorkScheduleRequest r) => api.PostAsync<WorkScheduleResponse>(ApiEndpoints.WorkSchedule.Create, r);
    public Task<ApiResponse<WorkScheduleResponse>> UpdateAsync(long id, UpdateWorkScheduleRequest r) => api.PutAsync<WorkScheduleResponse>(ApiEndpoints.WorkSchedule.Update(id), r);
    public Task<ApiResponse<WorkScheduleResponse>> DeleteAsync(long id) => api.PatchAsync<WorkScheduleResponse>(ApiEndpoints.WorkSchedule.Delete(id));
}

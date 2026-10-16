using BarberHub.Client.Core.Api;
using BarberHub.Client.Features.Account.Models;

namespace BarberHub.Client.Features.Account.Services;

/// <summary>UserController: پروفایل کاربر جاری (هر نقش)</summary>
public sealed class UserApi(ApiClient api)
{
    public Task<ApiResponse<UserResponse>> GetMeAsync() => api.GetAsync<UserResponse>(ApiEndpoints.User.Me);
    public Task<ApiResponse<UserResponse>> ChangePasswordAsync(ChangePasswordRequest r) => api.PatchAsync<UserResponse>(ApiEndpoints.User.ChangePassword, r);
}

/// <summary>EndUserController: GetAll/GetById برای PlatformAdmin، Update/Delete برای خود مشتری</summary>
public sealed class EndUserApi(ApiClient api)
{
    public Task<ApiResponse<List<EndUserResponse>>> GetAllAsync() => api.GetAsync<List<EndUserResponse>>(ApiEndpoints.EndUser.GetAll);
    public Task<ApiResponse<EndUserResponse>> GetByIdAsync(long id) => api.GetAsync<EndUserResponse>(ApiEndpoints.EndUser.GetById(id));
    public Task<ApiResponse<EndUserResponse>> UpdateMeAsync(UpdateEndUserRequest r) => api.PutAsync<EndUserResponse>(ApiEndpoints.EndUser.Update, r);
    public Task<ApiResponse<EndUserResponse>> DeleteMeAsync(long myId) => api.PatchAsync<EndUserResponse>(ApiEndpoints.EndUser.Delete(myId));
}

/// <summary>OtpController: تایید موبایل مشتری (محدودیت: ۳ ارسال / ۱۰ تایید در ۱۰ دقیقه)</summary>
public sealed class OtpApi(ApiClient api)
{
    public Task<ApiResponse> SendAsync() => api.PostAsync(ApiEndpoints.Otp.Send, null);
    public Task<ApiResponse> VerifyAsync(VerifyOtpRequest r) => api.PostAsync(ApiEndpoints.Otp.Verify, r);
}

using System.Net.Http.Headers;
using BarberHub.Client.Core.Api;
using BarberHub.Client.Features.Gallery.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace BarberHub.Client.Features.Gallery.Services;

/// <summary>GalleryController</summary>
public sealed class GalleryApi(ApiClient api)
{
    public const long MaxUploadBytes = 5 * 1024 * 1024; // سقف سمت کلاینت؛ سقف واقعی را سرور تعیین می‌کند

    public Task<ApiResponse<List<GalleryResponse>>> GetBySalonAsync(long salonId)
        => api.GetAsync<List<GalleryResponse>>(ApiEndpoints.Gallery.GetAllBySalon(salonId));
    public Task<ApiResponse<GalleryResponse>> GetByIdAsync(long id) => api.GetAsync<GalleryResponse>(ApiEndpoints.Gallery.GetById(id));

    /// <summary>[FromForm] CreateGalleryRequest: File, OriginFileName, Caption?, BarberId?</summary>
    public async Task<ApiResponse<GalleryResponse>> UploadAsync(IBrowserFile file, string? caption, long? barberId)
    {
        // فایل را یک بار در حافظه می‌خوانیم تا در retry (بعد از refresh توکن) هم قابل ارسال باشد
        await using var stream = file.OpenReadStream(MaxUploadBytes);
        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);
        var bytes = ms.ToArray();

        return await api.PostMultipartAsync<GalleryResponse>(ApiEndpoints.Gallery.Create, () =>
        {
            var content = new MultipartFormDataContent();
            var fileContent = new ByteArrayContent(bytes);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(string.IsNullOrEmpty(file.ContentType) ? "application/octet-stream" : file.ContentType);
            content.Add(fileContent, "File", file.Name);
            content.Add(new StringContent(file.Name), "OriginFileName");
            if (!string.IsNullOrWhiteSpace(caption)) content.Add(new StringContent(caption), "Caption");
            if (barberId is not null) content.Add(new StringContent(barberId.Value.ToString()), "BarberId");
            return content;
        });
    }

    public Task<ApiResponse<GalleryResponse>> UpdateCaptionAsync(long id, string? caption)
        => api.PatchAsync<GalleryResponse>(ApiEndpoints.Gallery.UpdateCaption(id), new UpdateGalleryCaptionRequest(caption));
    public Task<ApiResponse<GalleryResponse>> DeleteAsync(long id) => api.PatchAsync<GalleryResponse>(ApiEndpoints.Gallery.Delete(id));
}

/// <summary>FileController: فقط SalonAdmin. برای گرفتن URL تصویر گالری.</summary>
public sealed class FileApi(ApiClient api, ApiOptions options)
{
    public async Task<ApiResponse<FileResponse>> GetByIdAsync(long id)
    {
        var res = await api.GetAsync<FileResponse>(ApiEndpoints.File.GetById(id));
        if (res is { Success: true, Data: not null })
            return ApiResponse<FileResponse>.Ok(res.Data with { Url = options.ToAbsoluteUrl(res.Data.Url) });
        return res;
    }
}

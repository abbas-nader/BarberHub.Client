namespace BarberHub.Client.Features.Gallery.Models;

public sealed record GalleryResponse(long Id, string? Caption, long SalonId, long? BarberId, long FileId);

public sealed record UpdateGalleryCaptionRequest(string? Caption);

public sealed record FileResponse(long Id, string FileName, string OriginFileName, string Url, string ContentType, long Size);

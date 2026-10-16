namespace BarberHub.Client.Core.Models;

public sealed record PaginatedResponse<T>(
    IReadOnlyCollection<T> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages);

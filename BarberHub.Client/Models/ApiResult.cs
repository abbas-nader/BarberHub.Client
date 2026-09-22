namespace BarberHub.Client.Models;

public class ApiResult<T>
{
    public bool Success { get; set; }
    public int Status{ get; set; }
    public object? Error { get; set; }
    public T? Data { get; set; }
}
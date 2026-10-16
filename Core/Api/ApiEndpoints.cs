namespace BarberHub.Client.Core.Api;

/// <summary>
/// آینه‌ی دقیق *UriConstants.cs در BarberHub.Api. همه نسبت به api/v1/ هستند.
/// اگر مسیری در بک‌اند عوض شد، فقط همین فایل را آپدیت کن.
/// </summary>
public static class ApiEndpoints
{
    public static class Auth
    {
        public const string Login = "auth/login";
        public const string Register = "auth/register";
        public const string Refresh = "auth/refresh";
        public const string Revoke = "auth/revoke";
    }

    public static class User
    {
        public const string Me = "user/me";
        public const string ChangePassword = "user/change-password";
    }

    public static class EndUser
    {
        public const string GetAll = "end-user";
        public static string GetById(long id) => $"end-user/{id}";
        public const string Update = "end-user/update";
        // بک‌اند در route پارامتر {endUserId} دارد ولی از توکن استفاده می‌کند؛ id خود کاربر را می‌فرستیم.
        public static string Delete(long id) => $"end-user/delete/{id}";
    }

    public static class Otp
    {
        public const string Send = "otp/send";
        public const string Verify = "otp/verify";
    }

    public static class Salon
    {
        public const string GetAll = "salon";
        public static string Paginated(int page, int size) => $"salon/paginated?PageNumber={page}&PageSize={size}";
        public static string GetById(long id) => $"salon/{id}";
        public const string Create = "salon/create";
        public const string Update = "salon/update";
        public static string Delete(long id) => $"salon/delete/{id}";
        public static string Activate(long id) => $"salon/activate/{id}";
        public static string Deactivate(long id) => $"salon/deactivate/{id}";
        public const string UpdateDepositAmount = "salon/update-depositAmount";
    }

    public static class SalonAdmin
    {
        public static string GetAllBySalon(long salonId) => $"salon-admin/salon/{salonId}";
        public static string GetById(long id) => $"salon-admin/{id}";
        public const string Create = "salon-admin/create";
        public static string Update(long id) => $"salon-admin/update/{id}";
        public static string Delete(long id) => $"salon-admin/delete/{id}";
    }

    public static class Barber
    {
        public static string GetAllBySalon(long salonId) => $"barber/salon/{salonId}";
        public static string GetAllPublicBySalon(long salonId) => $"barber/public/salon/{salonId}";
        public static string GetById(long id) => $"barber/{id}";
        public const string Create = "barber/create";
        public static string Update(long id) => $"barber/update/{id}";
        public static string Delete(long id) => $"barber/delete/{id}";
        public static string Activate(long id) => $"barber/activate/{id}";
        public static string Deactivate(long id) => $"barber/deactivate/{id}";
    }

    public static class BarberService
    {
        public static string GetAllByBarber(long barberId) => $"barber-service/barber/{barberId}";
        public static string GetById(long id) => $"barber-service/{id}";
        public const string Create = "barber-service/create";
        public static string Update(long id) => $"barber-service/update/{id}";
        public static string Delete(long id) => $"barber-service/delete/{id}";
    }

    public static class Service
    {
        public static string GetAllBySalon(long salonId) => $"service/salon/{salonId}";
        public static string GetById(long id) => $"service/{id}";
        public const string Create = "service/create";
        public static string Update(long id) => $"service/update/{id}";
        public static string Delete(long id) => $"service/delete/{id}";
    }

    public static class WorkSchedule
    {
        public static string GetAllByBarber(long barberId) => $"workschedule/barber/{barberId}";
        public static string GetById(long id) => $"workschedule/{id}";
        public const string Create = "workschedule/create";
        public static string Update(long id) => $"workschedule/update/{id}";
        public static string Delete(long id) => $"workschedule/delete/{id}";
    }

    public static class Gallery
    {
        public static string GetAllBySalon(long salonId) => $"gallery/salon/{salonId}";
        public static string GetById(long id) => $"gallery/{id}";
        public const string Create = "gallery/create";
        public static string UpdateCaption(long id) => $"gallery/update-caption/{id}";
        public static string Delete(long id) => $"gallery/delete/{id}";
    }

    public static class File
    {
        public static string GetById(long id) => $"file/{id}";
    }

    public static class PlatformAdmin
    {
        public const string GetAll = "platform-admin";
        public static string GetById(long id) => $"platform-admin/{id}";
        public const string Create = "platform-admin/create";
        public const string Update = "platform-admin/update";
        public static string Delete(long id) => $"platform-admin/delete/{id}";
    }

    public static class ExceptionLog
    {
        public const string GetAll = "exceptionlog";
        public static string Recent(int count) => $"exceptionlog/recent?count={count}";
        public static string GetById(string id) => $"exceptionlog/{Uri.EscapeDataString(id)}";
    }
}

using System.Globalization;

namespace BarberHub.Client.Core.Models;

/// <summary>فرمت‌کننده‌های نمایشی فارسی، یک جا برای همه‌ی صفحه‌ها.</summary>
public static class Fmt
{
    private static readonly CultureInfo Fa = new("fa-IR");

    public static string Money(decimal value, Currency currency = Currency.Irr)
        => $"{value.ToString("#,0", Fa)} {CurrencyName(currency)}";

    public static string CurrencyName(Currency c) => c switch { Currency.Irr => "ریال", _ => c.ToString() };

    public static string Duration(TimeSpan d)
    {
        if (d.TotalMinutes < 60) return $"{(int)d.TotalMinutes} دقیقه";
        var h = (int)d.TotalHours; var m = d.Minutes;
        return m == 0 ? $"{h} ساعت" : $"{h} ساعت و {m} دقیقه";
    }

    public static string Time(TimeOnly t) => t.ToString("HH:mm", CultureInfo.InvariantCulture);

    public static string DateTime(DateTimeOffset d) => d.ToLocalTime().ToString("yyyy/MM/dd HH:mm", Fa);

    public static string Day(DayOfWeek d) => d switch
    {
        DayOfWeek.Saturday => "شنبه",
        DayOfWeek.Sunday => "یکشنبه",
        DayOfWeek.Monday => "دوشنبه",
        DayOfWeek.Tuesday => "سه‌شنبه",
        DayOfWeek.Wednesday => "چهارشنبه",
        DayOfWeek.Thursday => "پنجشنبه",
        DayOfWeek.Friday => "جمعه",
        _ => d.ToString()
    };

    /// <summary>ترتیب هفته‌ی ایرانی: شنبه تا جمعه</summary>
    public static readonly DayOfWeek[] PersianWeek =
    [
        DayOfWeek.Saturday, DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday,
        DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday
    ];

    public static string Role(UserRole r) => r switch
    {
        UserRole.PlatformAdmin => "مدیر پلتفرم",
        UserRole.SalonAdmin => "مدیر سالن",
        UserRole.Barber => "آرایشگر",
        UserRole.EndUser => "مشتری",
        _ => r.ToString()
    };

    public static string Initials(string first, string last)
        => $"{(first.Length > 0 ? first[0] : ' ')}{(last.Length > 0 ? last[0] : ' ')}".Trim();

    public static string FileSize(long bytes) => bytes switch
    {
        < 1024 => $"{bytes} B",
        < 1024 * 1024 => $"{bytes / 1024.0:0.#} KB",
        _ => $"{bytes / 1024.0 / 1024.0:0.#} MB"
    };
}

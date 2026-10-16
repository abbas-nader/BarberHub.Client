using System.ComponentModel.DataAnnotations;

namespace BarberHub.Client.Features.WorkSchedules.Models;

public sealed record WorkScheduleResponse(long Id, TimeOnly StartTime, TimeOnly EndTime, DayOfWeek DayOfWeek, long BarberId);

public sealed record CreateWorkScheduleRequest(TimeOnly StartTime, TimeOnly EndTime, DayOfWeek DayOfWeek, long BarberId);

public sealed record UpdateWorkScheduleRequest(TimeOnly StartTime, TimeOnly EndTime, DayOfWeek DayOfWeek);

public sealed class WorkScheduleForm : IValidatableObject
{
    public DayOfWeek DayOfWeek { get; set; } = DayOfWeek.Saturday;
    public TimeOnly StartTime { get; set; } = new(9, 0);
    public TimeOnly EndTime { get; set; } = new(18, 0);

    public IEnumerable<ValidationResult> Validate(ValidationContext _)
    {
        if (EndTime <= StartTime)
            yield return new ValidationResult("ساعت پایان باید بعد از ساعت شروع باشد", [nameof(EndTime)]);
    }
}

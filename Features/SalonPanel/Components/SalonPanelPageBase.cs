using BarberHub.Client.Core.Auth;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BarberHub.Client.Features.SalonPanel.Components;

/// <summary>
/// پایه‌ی صفحات پنل سالن: salonId را از claim «salonid» توکن مدیر سالن می‌خواند.
/// endpointهای ایجاد/ویرایش (barber/create، service/create، salon/update…) هم سالن را از همین claim تشخیص می‌دهند.
/// </summary>
public abstract class SalonPanelPageBase : ComponentBase
{
    [CascadingParameter] protected Task<AuthenticationState> AuthState { get; set; } = default!;

    protected long SalonId { get; private set; }
    protected bool MissingSalon { get; private set; }

    protected override async Task OnInitializedAsync()
    {
        var user = (await AuthState).User;
        var id = user.SalonId();
        if (id is null) { MissingSalon = true; return; }
        SalonId = id.Value;
        await LoadAsync();
    }

    protected abstract Task LoadAsync();
}

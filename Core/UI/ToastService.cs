namespace BarberHub.Client.Core.UI;

public enum ToastKind { Success, Error, Info }

public sealed record Toast(Guid Id, string Message, ToastKind Kind);

/// <summary>پیام‌های کوتاه موفقیت/خطا؛ ToastHost در MainLayout نمایش می‌دهد.</summary>
public sealed class ToastService
{
    private readonly List<Toast> _items = [];
    public IReadOnlyList<Toast> Items => _items;
    public event Action? Changed;

    public void Success(string m) => Show(m, ToastKind.Success);
    public void Error(string m) => Show(m, ToastKind.Error);
    public void Info(string m) => Show(m, ToastKind.Info);

    private async void Show(string message, ToastKind kind)
    {
        var t = new Toast(Guid.NewGuid(), message, kind);
        _items.Add(t); Changed?.Invoke();
        await Task.Delay(kind == ToastKind.Error ? 6000 : 3500);
        _items.Remove(t); Changed?.Invoke();
    }

    public void Dismiss(Guid id) { _items.RemoveAll(x => x.Id == id); Changed?.Invoke(); }
}

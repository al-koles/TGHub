using Microsoft.JSInterop;

namespace TGHub.Application.Common.DateAndTime;

public class LocalDateTimeManager : ILocalDateTimeManager
{
    private readonly IJSRuntime _jsRuntime;
    private TimeSpan _offset;

    public LocalDateTimeManager(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public TimeSpan Offset
    {
        get => _offset;
        set
        {
            _offset = value;
            OnOffsetChanged?.Invoke();
            OnOffsetChangedAsync?.Invoke();
        }
    }

    public event Action? OnOffsetChanged;
    public event Func<Task>? OnOffsetChangedAsync;

    public void SetOffsetByTimeZoneId(string timeZoneId)
    {
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        var offset = timeZone.GetUtcOffset(DateTime.UtcNow);

        Offset = TimeSpan.FromHours((int)offset.TotalHours);
    }

    public async Task SetFromJsAsync()
    {
        var offset = await _jsRuntime.InvokeAsync<int>("getClientTimeZoneOffset");
        Offset = TimeSpan.FromHours(offset);
    }

    public (DateOnly, TimeOnly) Convert(DateOnly date, TimeOnly time, To format)
    {
        var resultDateTime = date.ToDateTime(time) + Offset * (int)format;

        date = DateOnly.FromDateTime(resultDateTime);
        time = TimeOnly.FromDateTime(resultDateTime);

        return (date, time);
    }

    public DateTime Convert(DateTime dateTime, To format)
    {
        return dateTime + (int)format * Offset;
    }
}
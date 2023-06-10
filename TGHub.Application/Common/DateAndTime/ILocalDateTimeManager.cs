namespace TGHub.Application.Common.DateAndTime;

public enum To
{
    Utc = -1,
    Local = 1
}

public interface ILocalDateTimeManager
{
    event Action? OnOffsetChanged;
    event Func<Task>? OnOffsetChangedAsync;
    TimeSpan Offset { get; set; }
    void SetOffsetByTimeZoneId(string timeZoneId);
    Task SetFromJsAsync();
    (DateOnly, TimeOnly) ConvertDateAndTime(DateOnly date, TimeOnly time, To format);
    DateTime ConvertDateTime(DateTime dateTime, To format);
}
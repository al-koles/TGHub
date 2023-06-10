namespace TGHub.Application.Common.DateAndTime;

public enum To
{
    Utc = -1,
    Local = 1
}

public interface ILocalDateTimeManager
{
    TimeSpan Offset { get; set; }
    event Action? OnOffsetChanged;
    event Func<Task>? OnOffsetChangedAsync;
    void SetOffsetByTimeZoneId(string timeZoneId);
    Task SetFromJsAsync();
    (DateOnly, TimeOnly) Convert(DateOnly date, TimeOnly time, To format);
    DateTime Convert(DateTime dateTime, To format);
}
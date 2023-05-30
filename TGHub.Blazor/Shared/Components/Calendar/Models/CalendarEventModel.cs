namespace TGHub.Blazor.Shared.Components.Calendar.Models;

public class CalendarEventModel
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public DateTime ReleaseDateTime { get; set; }
    public CalendarEventStatus Status { get; set; }
}
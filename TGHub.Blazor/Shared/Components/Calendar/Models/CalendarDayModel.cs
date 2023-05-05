namespace TGHub.Blazor.Shared.Components.Calendar.Models;

public class CalendarDayModel
{
    public DateTime Date { get; set; }
    public List<CalendarEventModel> Events { get; set; } = new();
}
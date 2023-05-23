using TGHub.Domain.Enums;

namespace TGHub.Blazor.Shared.Components.YearMonthSelector;

public class YearMonthSelectorModel
{
    public int Year { get; set; } = DateTime.UtcNow.Year;
    public Month Month { get; set; } = (Month)DateTime.UtcNow.Month;
}
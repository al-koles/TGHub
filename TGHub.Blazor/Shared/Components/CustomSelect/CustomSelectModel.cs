namespace TGHub.Blazor.Shared.Components.CustomSelect;

public class CustomSelectModel<TType>
{
    public TType Id { get; set; }
    public string Name { get; set; } = null!;
    public string? PhotoUrl { get; set; }
}
namespace TGHub.Application.Common.LiveUpdates;

internal class Receiver : IDisposable
{
    internal Guid Id { get; set; }
    internal string[] Identifiers { get; set; } = null!;
    internal Func<Task> Handler { get; set; } = null!;

    public void Dispose()
    {
        OnDispose?.Invoke();
    }

    internal event Action? OnDispose;
}
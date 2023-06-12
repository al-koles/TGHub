namespace TGHub.Application.Common.LiveUpdates;

internal class LiveUpdatesHub : ILiveUpdatesHub
{
    private readonly List<Receiver> _receivers = new();

    public void SendUpdate(params string[] identifiers)
    {
        var receiversToUpdate = _receivers.Where(r => identifiers.All(i => r.Identifiers.Contains(i)));
        foreach (var receiver in receiversToUpdate)
        {
            receiver.Handler.Invoke();
        }
    }

    public IDisposable StartReceiving(Func<Task> handler, params string[] identifiers)
    {
        var receiver = new Receiver
        {
            Identifiers = identifiers,
            Handler = handler
        };
        receiver.OnDispose += () => _receivers.Remove(receiver);
        _receivers.Add(receiver);
        return receiver;
    }
}
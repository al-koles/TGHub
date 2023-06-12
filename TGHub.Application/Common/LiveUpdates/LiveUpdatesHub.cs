namespace TGHub.Application.Common.LiveUpdates;

internal class LiveUpdatesHub
{
    private readonly List<Receiver> _receivers = new();

    public void SendUpdate(Guid senderId, params string[] identifiers)
    {
        var receiversToUpdate = _receivers
            .Where(r => r.Id != senderId && identifiers.All(i => r.Identifiers.Contains(i)));
        foreach (var receiver in receiversToUpdate)
        {
            receiver.Handler.Invoke();
        }
    }

    public IDisposable StartReceiving(Guid receiverId, Func<Task> handler, params string[] identifiers)
    {
        var receiver = new Receiver
        {
            Id = receiverId,
            Identifiers = identifiers,
            Handler = handler
        };
        receiver.OnDispose += () => _receivers.Remove(receiver);
        _receivers.Add(receiver);
        return receiver;
    }
}
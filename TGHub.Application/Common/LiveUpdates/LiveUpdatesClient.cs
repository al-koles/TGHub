namespace TGHub.Application.Common.LiveUpdates;

internal class LiveUpdatesClient : ILiveUpdatesClient
{
    private readonly Guid _id = Guid.NewGuid();
    private readonly LiveUpdatesHub _liveUpdatesHub;

    public LiveUpdatesClient(LiveUpdatesHub liveUpdatesHub)
    {
        _liveUpdatesHub = liveUpdatesHub;
    }

    public void SendUpdate(params string[] identifiers)
    {
        _liveUpdatesHub.SendUpdate(_id, identifiers);
    }

    public IDisposable StartReceiving(Func<Task> handler, params string[] identifiers)
    {
        return _liveUpdatesHub.StartReceiving(_id, handler, identifiers);
    }
}
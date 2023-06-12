namespace TGHub.Application.Common.LiveUpdates;

public interface ILiveUpdatesHub
{
    void SendUpdate(params string[] identifiers);
    IDisposable StartReceiving(Func<Task> handler, params string[] identifiers);
}
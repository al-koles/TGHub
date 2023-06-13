namespace TGHub.Application.Common.LiveUpdates;

public interface ILiveUpdatesClient
{
    void SendUpdate(params string[] identifiers);
    IDisposable StartReceiving(Func<Task> handler, params string[] identifiers);
}
using TGHub.Application.Services.Base;
using TGHub.Application.Services.Spam.Models;
using TGHub.Domain.Entities;

namespace TGHub.Application.Services.Spam;

public interface ISpamService : IService<SpamMessage>
{
    Task<bool> BannSpammerIfOutOfLimitAsync(SpammerModel spammer, int spamMessagesLimit = 5);
    Task<List<SpamMessage>> GetLastSpamMessagesAsync(int channelId, long authorTgId);
    Task<List<string>> FindSpamWordsAsync(string text, int channelId);
}
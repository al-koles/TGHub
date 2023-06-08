using TGHub.Application.Services.Base;
using TGHub.Application.Services.Spam.Models;
using TGHub.Domain.Entities;

namespace TGHub.Application.Services.Spam;

public interface ISpamService : IService<SpamMessage>
{
    Task<Spammer> UpdateOrCreateSpammerAsync(SpammerModel spammerModel);
    Task<bool> BannSpammerIfOutOfLimitAsync(Spammer spammer, int spamMessagesLimit = 5);
    Task<List<SpamMessage>> GetLastSpamMessagesBySpammerAsync(Spammer spammer);
    Task<List<string>> FindSpamWordsAsync(string text, int channelId);
}
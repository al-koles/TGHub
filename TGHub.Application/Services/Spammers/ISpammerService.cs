using TGHub.Application.Services.Base;
using TGHub.Application.Services.Spammers.Models;
using TGHub.Domain.Entities;

namespace TGHub.Application.Services.Spammers;

public interface ISpammerService : IService<Spammer>
{
    Task<Spammer> UpdateOrCreateSpammerAsync(SpammerModel spammerModel);
    Task<bool> BannSpammerIfOutOfLimitAsync(Spammer spammer, int spamMessagesLimit = 5);
}
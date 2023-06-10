using TGHub.Application.Services.Base;
using TGHub.Domain.Entities;

namespace TGHub.Application.Services.SpamMessages;

public interface ISpamMessageService : IService<SpamMessage>
{
    Task<List<SpamMessage>> GetLastSpamMessagesBySpammerAsync(Spammer spammer);
}
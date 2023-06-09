using TGHub.Application.Services.Base;
using TGHub.Domain.Entities;

namespace TGHub.Application.Services.SpamWords;

public interface ISpamWordsService : IService<SpamWord>
{
    Task<List<string>> FindSpamWordsAsync(string text, int channelId);
}
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TGHub.Application;
using TGHub.Application.Interfaces;
using TGHub.Domain.Entities;
using TGHub.Domain.Enums;

namespace TGHub.Telegram.Bot.Lotteries;

internal class TgLotteryService : ITgLotteryService
{
    private readonly IFileStorage _fileStorage;
    private readonly ITelegramBotClient _telegramBotClient;

    public TgLotteryService(ITelegramBotClient telegramBotClient, IFileStorage fileStorage)
    {
        _telegramBotClient = telegramBotClient;
        _fileStorage = fileStorage;
    }

    public async Task<int> SendLotteryAsync(Lottery lottery)
    {
        var filesPath = Path.Combine(Constants.LotteryAttachmentsFolderName, lottery.AttachmentsFolderId.ToString());
        var attachments = lottery.Attachments.ToArray();
        var channelTgId = lottery.Creator.Channel.TelegramId;

        if (attachments.Length > 1)
        {
            return await SendAsMediaGroupAsync(lottery, filesPath);
        }

        if (lottery.Attachments.Count == 1)
        {
            return await SendAsSingleAttachmentAsync(lottery, filesPath);
        }

        var textMessage = await _telegramBotClient
            .SendTextMessageAsync(channelTgId, lottery.Content, replyMarkup: GetKeyboardMarkup(lottery));
        return textMessage.MessageId;
    }

    public async Task<int> SendResultAsync(Lottery lottery)
    {
        var channelTgId = lottery.Creator.Channel.TelegramId;
        var winners = lottery.Participants.Where(p => p.IsWinner).Select(p => p.NickName);

        var botName = await _telegramBotClient.GetMyNameAsync();
        var textMessage = await _telegramBotClient
            .SendTextMessageAsync(channelTgId,
                $"This lottery is over.{Environment.NewLine}" +
                $"Participants count: {lottery.Participants.Count}. Predefined winners count: {lottery.WinnersCount}.{Environment.NewLine}" +
                $"Winners: {string.Join(", ", winners.Select(w => '@' + w))}.{Environment.NewLine}" +
                $"Winners were selected randomly by {botName.Name}. " +
                $"if you are one of them, wait for the channel administrator to contact you.",
                replyToMessageId: lottery.LotteryTelegramId);
        return textMessage.MessageId;
    }

    private async Task<int> SendAsMediaGroupAsync(Lottery lottery, string filesPath)
    {
        var attachments = lottery.Attachments.ToArray();
        var channelTgId = lottery.Creator.Channel.TelegramId;

        var media = new List<IAlbumInputMedia>();
        var streams = new List<Stream>();

        try
        {
            if (lottery.AttachmentsFormat == MediaGroupFormat.PhotoVideo)
            {
                var isCaptionSet = false;
                foreach (var attachment in attachments)
                {
                    var stream = await _fileStorage.DownloadAsync(attachment.FileName, filesPath);
                    streams.Add(stream);
                    var inputFileStream = new InputFileStream(stream, attachment.FileName);

                    InputMedia? inputMedia;
                    switch (attachment.Type)
                    {
                        case AttachmentType.Photo:
                            var inputPhoto = new InputMediaPhoto(inputFileStream);
                            media.Add(inputPhoto);
                            inputMedia = inputPhoto;
                            break;
                        default:
                            var inputVideo = new InputMediaVideo(inputFileStream);
                            media.Add(inputVideo);
                            inputMedia = inputVideo;
                            break;
                    }

                    if (!isCaptionSet)
                    {
                        inputMedia.Caption = lottery.Content;
                        isCaptionSet = true;
                    }
                }
            }
            else if (lottery.AttachmentsFormat == MediaGroupFormat.Audio)
            {
                var inputAudios = new List<InputMediaAudio>();
                foreach (var attachment in attachments)
                {
                    var stream = await _fileStorage.DownloadAsync(attachment.FileName, filesPath);
                    streams.Add(stream);

                    var inputAudio = new InputMediaAudio(new InputFileStream(stream, attachment.FileName));
                    media.Add(inputAudio);
                    inputAudios.Add(inputAudio);
                }

                inputAudios.Last().Caption = lottery.Content;
            }
            else
            {
                var inputDocuments = new List<InputMediaDocument>();
                foreach (var attachment in attachments)
                {
                    var stream = await _fileStorage.DownloadAsync(attachment.FileName, filesPath);
                    streams.Add(stream);

                    var inputDocument = new InputMediaDocument(new InputFileStream(stream, attachment.FileName));
                    media.Add(inputDocument);
                    inputDocuments.Add(inputDocument);
                }

                inputDocuments.Last().Caption = lottery.Content;
            }

            var mediaGroup = await _telegramBotClient.SendMediaGroupAsync(channelTgId, media);
            await _telegramBotClient.SendTextMessageAsync(channelTgId, ".",
                replyToMessageId: mediaGroup.First().MessageId, replyMarkup: GetKeyboardMarkup(lottery));

            return mediaGroup.First().MessageId;
        }
        finally
        {
            await Task.WhenAll(streams.Select(async s => { await s.DisposeAsync(); }));
        }
    }

    private async Task<int> SendAsSingleAttachmentAsync(Lottery lottery, string filesPath)
    {
        var attachment = lottery.Attachments.First();
        var channelTgId = lottery.Creator.Channel.TelegramId;
        await using var stream = await _fileStorage.DownloadAsync(attachment.FileName, filesPath);
        var inputFileStream = new InputFileStream(stream, attachment.FileName);

        switch (lottery.AttachmentsFormat)
        {
            case MediaGroupFormat.PhotoVideo:
                switch (attachment.Type)
                {
                    case AttachmentType.Photo:
                        var photoMessage =
                            await _telegramBotClient.SendPhotoAsync(channelTgId, inputFileStream,
                                caption: lottery.Content, replyMarkup: GetKeyboardMarkup(lottery));
                        return photoMessage.MessageId;
                    default:
                        var videoMessage =
                            await _telegramBotClient.SendVideoAsync(channelTgId, inputFileStream,
                                caption: lottery.Content, replyMarkup: GetKeyboardMarkup(lottery));
                        return videoMessage.MessageId;
                }
            case MediaGroupFormat.Audio:
                var audioMessage =
                    await _telegramBotClient.SendAudioAsync(channelTgId, inputFileStream, caption: lottery.Content,
                        replyMarkup: GetKeyboardMarkup(lottery));
                return audioMessage.MessageId;
            default:
                var documentMessage =
                    await _telegramBotClient.SendDocumentAsync(channelTgId, inputFileStream, caption: lottery.Content,
                        replyMarkup: GetKeyboardMarkup(lottery));
                return documentMessage.MessageId;
        }
    }

    private InlineKeyboardMarkup GetKeyboardMarkup(Lottery lottery)
    {
        return new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData("Participate", lottery.Id.ToString()));
    }
}
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TGHub.Application;
using TGHub.Application.Interfaces;
using TGHub.Domain.Entities;
using TGHub.Domain.Enums;

namespace TGHub.Telegram.Bot.Posts;

internal class TgPostService : ITgPostService
{
    private readonly IFileStorage _fileStorage;
    private readonly ITelegramBotClient _telegramBotClient;

    public TgPostService(ITelegramBotClient telegramBotClient, IFileStorage fileStorage)
    {
        _telegramBotClient = telegramBotClient;
        _fileStorage = fileStorage;
    }

    public async Task<int> SendAsync(Post post)
    {
        var filesPath = Path.Combine(Constants.PostAttachmentsFolderName, post.AttachmentsFolderId.ToString());
        var attachments = post.Attachments.ToArray();
        var channelTgId = post.Creator.Channel.TelegramId;

        if (attachments.Length > 1)
        {
            return await SendAsMediaGroupAsync(post, filesPath);
        }

        if (post.Attachments.Count == 1)
        {
            return await SendAsSingleAttachmentAsync(post, filesPath);
        }

        var textMessage = await _telegramBotClient
            .SendTextMessageAsync(channelTgId, post.Content, replyMarkup: GetKeyboardMarkup(post));
        return textMessage.MessageId;
    }

    private async Task<int> SendAsMediaGroupAsync(Post post, string filesPath)
    {
        var attachments = post.Attachments.ToArray();
        var channelTgId = post.Creator.Channel.TelegramId;

        var media = new List<IAlbumInputMedia>();
        var streams = new List<Stream>();

        try
        {
            if (post.AttachmentsFormat == MediaGroupFormat.PhotoVideo)
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
                        inputMedia.Caption = post.Content;
                        isCaptionSet = true;
                    }
                }
            }
            else if (post.AttachmentsFormat == MediaGroupFormat.Audio)
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

                inputAudios.Last().Caption = post.Content;
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

                inputDocuments.Last().Caption = post.Content;
            }

            var mediaGroup = await _telegramBotClient.SendMediaGroupAsync(channelTgId, media);
            if (post.Buttons.Any())
            {
                await _telegramBotClient.SendTextMessageAsync(channelTgId, ".",
                    replyToMessageId: mediaGroup.First().MessageId, replyMarkup: GetKeyboardMarkup(post));
            }

            return mediaGroup.First().MessageId;
        }
        finally
        {
            await Task.WhenAll(streams.Select(async s => { await s.DisposeAsync(); }));
        }
    }

    private async Task<int> SendAsSingleAttachmentAsync(Post post, string filesPath)
    {
        var attachment = post.Attachments.First();
        var channelTgId = post.Creator.Channel.TelegramId;
        await using var stream = await _fileStorage.DownloadAsync(attachment.FileName, filesPath);
        var inputFileStream = new InputFileStream(stream, attachment.FileName);

        switch (post.AttachmentsFormat)
        {
            case MediaGroupFormat.PhotoVideo:
                switch (attachment.Type)
                {
                    case AttachmentType.Photo:
                        var photoMessage =
                            await _telegramBotClient.SendPhotoAsync(channelTgId, inputFileStream,
                                caption: post.Content, replyMarkup: GetKeyboardMarkup(post));
                        return photoMessage.MessageId;
                    default:
                        var videoMessage =
                            await _telegramBotClient.SendVideoAsync(channelTgId, inputFileStream,
                                caption: post.Content, replyMarkup: GetKeyboardMarkup(post));
                        return videoMessage.MessageId;
                }
            case MediaGroupFormat.Audio:
                var audioMessage =
                    await _telegramBotClient.SendAudioAsync(channelTgId, inputFileStream, caption: post.Content,
                        replyMarkup: GetKeyboardMarkup(post));
                return audioMessage.MessageId;
            default:
                var documentMessage =
                    await _telegramBotClient.SendDocumentAsync(channelTgId, inputFileStream, caption: post.Content,
                        replyMarkup: GetKeyboardMarkup(post));
                return documentMessage.MessageId;
        }
    }

    private InlineKeyboardMarkup GetKeyboardMarkup(Post post)
    {
        return new InlineKeyboardMarkup(
            post.Buttons.Select(b => new[] { InlineKeyboardButton.WithUrl(b.Content, b.Link) }));
    }
}
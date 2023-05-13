using Telegram.Bot;
using Telegram.Bot.Types;
using TGHub.Application;
using TGHub.Application.Interfaces;
using TGHub.Domain.Entities;
using TGHub.Domain.Enums;

namespace TGHub.Telegram.Bot.Posts;

internal class TgSendService : ITgSendService
{
    private readonly IFileStorage _fileStorage;
    private readonly ITelegramBotClient _telegramBotClient;

    public TgSendService(ITelegramBotClient telegramBotClient, IFileStorage fileStorage)
    {
        _telegramBotClient = telegramBotClient;
        _fileStorage = fileStorage;
    }

    public async Task<long> SendPostAsync(Post post)
    {
        var pathBase = Path.Combine(Constants.PostAttachmentsFolderName, post.Id.ToString());
        var attachments = post.Attachments.ToArray();
        var channelTgId = post.Creator.Channel.TelegramId;
        long messageId;

        if (attachments.Length > 1)
        {
            var media = new List<IAlbumInputMedia>();
            var streams = new List<Stream>();
            for (var i = 0; i < attachments.Length; i++)
            {
                var stream = await _fileStorage.DownloadAsync(attachments[i].FileName, pathBase);
                streams.Add(stream);
                var inputFileStream = new InputFileStream(stream, attachments[i].FileName);

                InputMedia? inputMedia = null;
                switch (attachments[i].Type)
                {
                    case AttachmentType.Photo:
                        var inputPhoto = new InputMediaPhoto(inputFileStream);
                        media.Add(inputPhoto);
                        inputMedia = inputPhoto;
                        break;
                    case AttachmentType.Video:
                        var inputVideo = new InputMediaVideo(inputFileStream);
                        media.Add(inputVideo);
                        inputMedia = inputVideo;
                        break;
                    case AttachmentType.Document:
                        var inputDoc = new InputMediaDocument(inputFileStream);
                        media.Add(inputDoc);
                        inputMedia = inputDoc;
                        break;
                }

                if (i == 0 && inputMedia != null)
                {
                    inputMedia.Caption = post.Content;
                }
            }

            var mediaGroup = await _telegramBotClient.SendMediaGroupAsync(channelTgId, media);
            messageId = mediaGroup.First().MessageId;

            foreach (var stream in streams)
            {
                await stream.DisposeAsync();
            }

            var streamDisposeTasks = streams.Select(async s => { await s.DisposeAsync(); }).ToList();
            await Task.WhenAll(streamDisposeTasks);
        }
        else if (post.Attachments.Count == 1)
        {
            var attachment = post.Attachments.First();
            await using var stream = await _fileStorage.DownloadAsync(attachment.FileName, pathBase);
            var inputFileStream = new InputFileStream(stream, attachment.FileName);

            switch (attachment.Type)
            {
                case AttachmentType.Photo:
                    var photoMessage =
                        await _telegramBotClient.SendPhotoAsync(channelTgId, inputFileStream, caption: post.Content);
                    messageId = photoMessage.MessageId;
                    break;
                case AttachmentType.Video:
                    var videoMessage =
                        await _telegramBotClient.SendVideoAsync(channelTgId, inputFileStream, caption: post.Content);
                    messageId = videoMessage.MessageId;
                    break;
                case AttachmentType.Document:
                    var documentMessage =
                        await _telegramBotClient.SendDocumentAsync(channelTgId, inputFileStream, caption: post.Content);
                    messageId = documentMessage.MessageId;
                    break;
                default:
                    messageId = 0;
                    break;
            }
        }
        else
        {
            var textMessage = await _telegramBotClient.SendTextMessageAsync(channelTgId, post.Content);
            messageId = textMessage.MessageId;
        }

        return messageId;
    }
}
﻿using TGHub.Domain.Enums;

namespace TGHub.Blazor.Data;

public static class AttachmentFormats
{
    public const string PhotoFormats = ".jpeg,.jpg,.png";
    public const string VideoFormats = ".mp4";
    public const string PhotoVideoFormats = ".jpeg,.jpg,.png,.mp4";
    public const string AudioFormats = ".mp3";
    public const string DocumentFormats = "";

    public static string GetFileFormats(this MediaGroupFormat mediaGroupFormat)
    {
        return mediaGroupFormat switch
        {
            MediaGroupFormat.PhotoVideo => PhotoVideoFormats,
            MediaGroupFormat.Audio => AudioFormats,
            _ => DocumentFormats
        };
    }

    public static MediaGroupFormat GetMediaGroupFormat(string fileFormat)
    {
        if (PhotoVideoFormats.Contains(fileFormat))
        {
            return MediaGroupFormat.PhotoVideo;
        }

        if (AudioFormats.Contains(fileFormat))
        {
            return MediaGroupFormat.Audio;
        }

        return MediaGroupFormat.Document;
    }

    public static MediaGroupFormat GetMediaGroupFormat(AttachmentType contentType)
    {
        return contentType switch
        {
            AttachmentType.Photo or AttachmentType.Video => MediaGroupFormat.PhotoVideo,
            AttachmentType.Audio => MediaGroupFormat.Audio,
            _ => MediaGroupFormat.Document
        };
    }

    public static AttachmentType GetAttachmentType(string fileFormat)
    {
        if (PhotoFormats.Contains(fileFormat))
        {
            return AttachmentType.Photo;
        }

        if (VideoFormats.Contains(fileFormat))
        {
            return AttachmentType.Video;
        }

        if (AudioFormats.Contains(fileFormat))
        {
            return AttachmentType.Audio;
        }

        return AttachmentType.Document;
    }
}
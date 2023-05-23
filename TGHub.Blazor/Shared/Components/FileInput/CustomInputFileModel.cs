using Microsoft.AspNetCore.Components.Forms;
using TGHub.Domain.Enums;

namespace TGHub.Blazor.Shared.Components.FileInput;

public class CustomInputFileModel
{
    public IBrowserFile File { get; set; } = null!;
    public AttachmentType Format { get; set; }
    public FileUploadStatus UploadStatus { get; set; }
    public long BytesUploaded { get; set; }
    public CancellationTokenSource UploadCancellationTokenSource { get; set; } = new();
    public string? Url { get; set; }
}
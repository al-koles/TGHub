namespace TGHub.Domain.Enums;

[Flags]
public enum SpamMessageType
{
    OffensiveLanguage = 1 << 0,
    SpamWordFound = 1 << 1
}
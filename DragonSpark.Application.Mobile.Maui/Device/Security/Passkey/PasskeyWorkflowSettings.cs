namespace DragonSpark.Application.Mobile.Maui.Device.Security.Passkey;

public sealed record PasskeyWorkflowSettings
{
    public required Uri Address { get; init; }

    public required string Register { get; init; } = "passkey/register";

    public required string Login { get; init; } = "passkey/login";
}
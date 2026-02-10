namespace DragonSpark.Server.Mobile.Security.Devices.Validation;

public sealed record DeviceValidationSettings
{
    public bool IncludeAttestation { get; init; } = true;
}
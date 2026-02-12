using Google.Apis.PlayIntegrity.v1.Data;

namespace DragonSpark.Server.Mobile.Platforms.Android.Attestation;

public sealed record IntegrityTokenResult(
    RequestDetails Request,
    ApplicationIntegrity Application,
    DeviceIntegrity Device)
{
    public IntegrityTokenResult(TokenPayloadExternal payload)
        : this(payload.RequestDetails, new(payload.AppIntegrity.AppRecognitionVerdict),
               new(payload.DeviceIntegrity.DeviceRecognitionVerdict)) {}
}
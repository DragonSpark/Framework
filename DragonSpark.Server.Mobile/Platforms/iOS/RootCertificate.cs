using DragonSpark.Model.Sequences;
using DragonSpark.Server.Mobile.Properties;

namespace DragonSpark.Server.Mobile.Platforms.iOS;

sealed class RootCertificate : Instances<byte>
{
    public static RootCertificate Default { get; } = new();

    RootCertificate() : base(Resources.Apple_App_Attestation_Root_CA_pem) {}
}
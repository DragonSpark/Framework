using Security;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Security.Tokens;

sealed class SecurityKey : DragonSpark.Model.Results.Instance<SecKey>
{
    public static SecurityKey Default { get; } = new();

    SecurityKey() : this(SecurityRecord.Default) {}

    public SecurityKey(SecRecord record) : base(DetermineSecurityKey.Default.Get(record)) {}
}
using DragonSpark.Model.Results;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Assertion;

sealed class AuthenticationDataLength : Instance<byte>
{
    public static AuthenticationDataLength Default { get; } = new();

    AuthenticationDataLength() : base(37) {}
}
using DragonSpark.Model.Selection;
using Security;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Security.Tokens;

sealed class ExistingSecurityKey : ISelect<SecRecord, SecKey?>
{
    public static ExistingSecurityKey Default { get; } = new();

    ExistingSecurityKey() {}

    public SecKey? Get(SecRecord parameter)
    {
        var existing = SecKeyChain.QueryAsConcreteType(parameter, out var status);
        return status == SecStatusCode.Success && existing is SecKey key ? key : null;
    }
}
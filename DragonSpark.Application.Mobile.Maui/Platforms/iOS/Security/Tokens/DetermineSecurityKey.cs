using DragonSpark.Model.Selection;
using Security;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Security.Tokens;

sealed class DetermineSecurityKey : Coalesce<SecRecord, SecKey>
{
    public static DetermineSecurityKey Default { get; } = new();

    DetermineSecurityKey() : base(ExistingSecurityKey.Default, CreateSecurityKey.Default) {}
}
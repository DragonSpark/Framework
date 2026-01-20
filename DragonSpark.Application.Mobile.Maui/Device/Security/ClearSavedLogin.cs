using DragonSpark.Application.Model.Values;
using DragonSpark.Contracts.Security;

namespace DragonSpark.Application.Mobile.Maui.Device.Security;

sealed class ClearSavedLogin : ClearState<AccessTokenView>
{
    public static ClearSavedLogin Default { get; } = new();

    ClearSavedLogin() : base(SavedLogin.Default) {}
}
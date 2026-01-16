using DragonSpark.Application.Communication.Http.Security;
using DragonSpark.Application.Model.Values;

namespace DragonSpark.Application.Mobile.Maui.Device.Security;

sealed class ClearSavedLogin : ClearState<AccessTokenView>
{
    public static ClearSavedLogin Default { get; } = new();

    ClearSavedLogin() : base(SavedLogin.Default) {}
}
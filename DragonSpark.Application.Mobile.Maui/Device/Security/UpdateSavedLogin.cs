using DragonSpark.Application.Model.Values;

namespace DragonSpark.Application.Mobile.Maui.Device.Security;

public sealed class UpdateSavedLogin : UpdateState<string>, IUpdateSavedLogin
{
    public static UpdateSavedLogin Default { get; } = new();

    UpdateSavedLogin() : base(SavedLogin.Default, ClearSavedLogin.Default) {}
}
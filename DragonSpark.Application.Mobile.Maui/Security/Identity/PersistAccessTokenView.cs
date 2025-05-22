namespace DragonSpark.Application.Mobile.Maui.Security.Identity;

sealed class PersistAccessTokenView : DragonSpark.Application.Communication.Http.Security.PersistAccessTokenView
{
    public static PersistAccessTokenView Default { get; } = new();

    PersistAccessTokenView() : base(AccessTokenViewStorage.Default) {}
}
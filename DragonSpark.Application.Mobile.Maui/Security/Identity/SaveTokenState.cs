namespace DragonSpark.Application.Mobile.Maui.Security.Identity;

sealed class SaveTokenState : DragonSpark.Application.Communication.Http.Security.SaveTokenState
{
    public static SaveTokenState Default { get; } = new();

    SaveTokenState() : base(AccessTokenProcessValue.Default, AccessTokenViewStorage.Default) {}
}
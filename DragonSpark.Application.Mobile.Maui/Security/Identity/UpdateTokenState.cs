namespace DragonSpark.Application.Mobile.Maui.Security.Identity;

public sealed class UpdateTokenState : Communication.Http.Security.UpdateTokenState
{
    public static UpdateTokenState Default { get; } = new();

    UpdateTokenState() : base(SaveTokenState.Default, ClearTokenState.Default) {}
}
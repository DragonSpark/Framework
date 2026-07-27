using DragonSpark.Application.Communication.Http.Security;
using DragonSpark.Compose;
using DragonSpark.Contracts.Security;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Mobile.Maui.Device.Security;

sealed class SavedLoginAwareCompleteLogin : ICompleteLogin
{
    readonly ICompleteLogin    _previous;
    readonly IUpdateSavedLogin _update;

    public SavedLoginAwareCompleteLogin(ICompleteLogin previous) : this(previous, UpdateSavedLogin.Default) {}

    public SavedLoginAwareCompleteLogin(ICompleteLogin previous, IUpdateSavedLogin update)
    {
        _previous = previous;
        _update   = update;
    }

    public async ValueTask Get(Stop<AccessTokenView?> parameter)
    {
        var (subject, stop) = parameter;
        await _previous.Off(parameter);
        await _update.Off(new(subject?.Identifier, stop));
    }
}
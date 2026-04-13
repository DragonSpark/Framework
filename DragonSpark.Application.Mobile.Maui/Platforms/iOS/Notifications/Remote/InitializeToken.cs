using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Notifications.Remote;

sealed class InitializeToken : ICommand
{
    public static InitializeToken Default { get; } = new();

    InitializeToken() : this(IsSupported.Default, Register.Default) {}

    readonly ICondition _supported;
    readonly ICommand   _register;

    public InitializeToken(ICondition supported, ICommand register)
    {
        _supported = supported;
        _register  = register;
    }

    public void Execute(None parameter)
    {
        if (_supported.Get())
        {
            _register.Execute();
        }
    }
}
using System.Windows.Input;
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Text;

namespace DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote.Messages;

sealed class ActionParser : IActionParser
{
    readonly IParser<ActionParameter>                  _parser;
    readonly IConditional<string, IActionRegistration> _registrations;
    
    public ActionParser(ActionRegistrations registrations) : this(ActionParameterParser.Default, registrations) {}

    public ActionParser(IParser<ActionParameter> parser, IConditional<string, IActionRegistration> registrations)
    {
        _parser        = parser;
        _registrations = registrations;
    }

    public ICommand? Get(string parameter)
    {
        var (name, argument) = _parser.Get(parameter);
        return _registrations.TryGet(name, out var result) ? result.Get(argument) : null;
    }
}
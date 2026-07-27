using DragonSpark.Text;

namespace DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote.Messages;

sealed class ActionParameterParser : IParser<ActionParameter>
{
    public static ActionParameterParser Default { get; } = new();

    ActionParameterParser() : this(':') {}

    readonly char _split;

    public ActionParameterParser(char split) => _split = split;

    public ActionParameter Get(string parameter)
    {
        var parts = parameter.Split(_split);
        return new(parts[0], parts.Length > 1 ? string.Join(_split, parts.Skip(1)) : null);
    }
}
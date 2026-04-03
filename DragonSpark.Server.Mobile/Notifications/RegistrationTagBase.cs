using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.Azure.NotificationHubs;

namespace DragonSpark.Server.Mobile.Notifications;

class RegistrationTagBase : IRegistrationTag
{
    readonly string                                                         _name;
    readonly ISelect<RegistrationDescription, IConditional<string, string>> _tags;

    protected RegistrationTagBase(string name) : this(name, RegistrationTags.Default) {}

    protected RegistrationTagBase(string name, ISelect<RegistrationDescription, IConditional<string, string>> tags)
    {
        _name = name;
        _tags = tags;
    }

    public string? Get(RegistrationDescription parameter)
        => _tags.Get(parameter).TryGet(_name, out var result) ? result : null;
}
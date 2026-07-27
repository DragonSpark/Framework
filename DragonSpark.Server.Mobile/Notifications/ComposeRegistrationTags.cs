using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.Azure.NotificationHubs;

namespace DragonSpark.Server.Mobile.Notifications;

sealed class ComposeRegistrationTags : Select<RegistrationDescription, IConditional<string, string>>
{
    public static ComposeRegistrationTags Default { get; } = new();

    ComposeRegistrationTags()
        : base(x => x.Tags.Select(y => y.Split(':')).ToDictionary(y => y[0], y => y[1]).ToStore()) {}
}
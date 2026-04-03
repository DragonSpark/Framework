using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Selection.Stores;
using Microsoft.Azure.NotificationHubs;

namespace DragonSpark.Server.Mobile.Notifications;

sealed class RegistrationTags : ReferenceValueStore<RegistrationDescription, IConditional<string, string>>
{
    public static RegistrationTags Default { get; } = new();

    RegistrationTags() : base(ComposeRegistrationTags.Default) {}
}
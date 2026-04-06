using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Microsoft.Azure.NotificationHubs;

namespace DragonSpark.Server.Mobile.Notifications;

sealed class RegistrationInstallation : Select<RegistrationDescription, string?>
{
    public static RegistrationInstallation Default { get; } = new();

    RegistrationInstallation() : base(RegistrationInstallationTag.Default.Then().Select(x => x?.Trim('{', '}'))) {}
}
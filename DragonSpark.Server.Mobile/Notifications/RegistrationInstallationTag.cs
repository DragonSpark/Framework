namespace DragonSpark.Server.Mobile.Notifications;

sealed class RegistrationInstallationTag : RegistrationTagBase
{
    public static RegistrationInstallationTag Default { get; } = new();

    RegistrationInstallationTag() : base("$InstallationId") {}
}
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote.Messages;

sealed class ActionRegistrations : Assume<string, IActionRegistration>
{
    public ActionRegistrations(ComposeRegistrations previous) : base(previous.Then().Singleton()) {}
}
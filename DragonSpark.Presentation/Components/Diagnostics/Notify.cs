using DragonSpark.Model.Commands;
using Radzen;

namespace DragonSpark.Presentation.Components.Diagnostics;

sealed class Notify : Command<NotificationMessage>
{
	public Notify(NotificationService service) : base(service.Notify) {}
}
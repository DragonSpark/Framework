using Azure.Messaging.ServiceBus;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Azure.Messaging.Messages.Topics.Receive;

public sealed class ContainsIntendedAudience : Condition<ProcessMessageEventArgs>
{
	public ContainsIntendedAudience(string? audience)
		: base(new Messaging.Receive.ContainsIntendedAudience(audience).Get) {}
}
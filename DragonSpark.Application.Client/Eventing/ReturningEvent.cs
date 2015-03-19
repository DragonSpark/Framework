using Prism.Events;
using System.ComponentModel;

namespace DragonSpark.Application.Client.Eventing
{
	public class ReturningEvent : PrismEvent<CancelEventArgs>
	{}

	public class ReturnEvent : PrismEvent<object>
	{}
}
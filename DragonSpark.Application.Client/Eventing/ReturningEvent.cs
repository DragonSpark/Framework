using System.ComponentModel;
using Microsoft.Practices.Prism.PubSubEvents;
using Xamarin.Forms;

namespace DragonSpark.Application.Client.Eventing
{
	public class ReturningEvent : PubSubEvent<CancelEventArgs>
	{}

	public class ReturnEvent : PubSubEvent<object>
	{}
}
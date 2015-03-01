using Microsoft.Practices.Prism.PubSubEvents;
using Xamarin.Forms;

namespace DragonSpark.Application.Client.Eventing
{
	public class ShellPageChangedEvent : PubSubEvent<Page>
	{}

	public class ShellSizeChangedEvent : PubSubEvent<Size>
	{}

	public class ShellScaleFactorChangedEvent : PubSubEvent<double>
	{}
}
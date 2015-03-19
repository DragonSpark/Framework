using Prism.Events;
using Xamarin.Forms;

namespace DragonSpark.Application.Client.Eventing
{
	public class ShellPageChangedEvent : PrismEvent<Page>
	{}

	public class ShellSizeChangedEvent : PrismEvent<Size>
	{}

	public class ShellScaleFactorChangedEvent : PrismEvent<double>
	{}
}
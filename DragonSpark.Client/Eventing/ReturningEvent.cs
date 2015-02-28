using System.ComponentModel;
using Xamarin.Forms;

namespace DragonSpark.Application.Client.Eventing
{
	public class ReturningEvent : FormsEvent<Page, CancelEventArgs>
	{}

	public class ReturnEvent : FormsEvent<Page>
	{ }

	
}
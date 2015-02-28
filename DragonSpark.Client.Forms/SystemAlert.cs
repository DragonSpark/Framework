using DragonSpark.Application.Client.Eventing;
using DragonSpark.Application.Client.Interaction;
using DragonSpark.Extensions;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Xamarin.Forms;

namespace DragonSpark.Application.Client.Forms
{
	[MessageName( "Xamarin.SendAlert" )]
	class DialogAlertEvent : FormsEvent<Page, AlertArguments>
	{ }

	public class SystemAlert : InteractionRequest<DialogNotification>
	{
		public SystemAlert()
		{
			this.Event<DialogAlertEvent>().Subscribe( this, OnAlert );
		}

		~SystemAlert()
		{
			this.Event<DialogAlertEvent>().Unsubscribe( this );
		}

		public void Test()
		{
			this.Event<DialogAlertEvent>().Publish( new ContentPage(), new AlertArguments( "Testing 12 3", "This is a Test Message", "OK!!!", "CANELLLLLLLLL" ) );
		}

		void OnAlert( Page page, AlertArguments arguments )
		{
			var notification = arguments.MapInto<DialogNotification>().With( item => item.Content = arguments.Message );
			Raise( notification, item => arguments.SetResult( item.Result.GetValueOrDefault() )  );

			/*var dialog = new ModernDialog
			{
				Title = arguments.Title,
				Content = arguments.Message
			};

			arguments.Accept.With( accept => dialog.OkButton.Content = accept );
			dialog.CancelButton.Content = arguments.Cancel;
			dialog.Buttons = new[] { arguments.Accept != null ? dialog.OkButton : null, dialog.CancelButton }.NotNull().ToArray();
			dialog.Show();

			ShellProperties.SetDialog( shell, dialog );

			dialog.Closed += ( o, args ) =>
			{
				arguments.SetResult( dialog.MessageBoxResult == MessageBoxResult.OK );
				ShellProperties.SetDialog( shell, null );
			};*/
		}
	}
}
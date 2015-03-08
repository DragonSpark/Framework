using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

namespace DragonSpark.Application.Presentation.Commands
{
	public class DialogCommand : DialogCommand<Notification,Notification>
	{
		/*protected override void Execute( object parameter )
		{
			var content = DetermineContent();
			
		}*/
		protected override void Execute( Notification parameter )
		{
			// var notification = new Notification { Title = Title, Content = parameter };
			parameter.Content = parameter.Content ?? parameter;
			DisplayRequest.Raise( parameter );
		}

		/*protected virtual object DetermineContent()
		{
			var result = Activator.Create<TContent>();
			return result;
		}*/
	}

	public abstract class DialogCommand<TParameter, TNotification> : CommandBase<TParameter> where TNotification : Notification
	{
		public virtual string Title
		{
			get { return title; }
			set { SetProperty( ref title, value, () => Title ); }
		}	string title;

		public InteractionRequest<TNotification> DisplayRequest
		{
			get { return displayRequest; }
		}	readonly InteractionRequest<TNotification> displayRequest = new InteractionRequest<TNotification>();
	}
}
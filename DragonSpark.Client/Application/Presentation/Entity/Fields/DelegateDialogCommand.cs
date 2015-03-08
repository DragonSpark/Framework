using System;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

namespace DragonSpark.Application.Presentation.Entity.Fields
{
	public abstract class DelegateDialogCommand<TParameter, TNotification> : DelegateCommand<TParameter> where TNotification : Notification
	{
		protected DelegateDialogCommand( Action<TParameter> executeMethod ) : base( executeMethod )
		{}

		protected DelegateDialogCommand( Action<TParameter> executeMethod, Func<TParameter, bool> canExecuteMethod ) : base( executeMethod, canExecuteMethod )
		{}

		public InteractionRequest<TNotification> DisplayRequest
		{
			get { return displayRequest; }
		}	readonly InteractionRequest<TNotification> displayRequest = new InteractionRequest<TNotification>();
	}
}
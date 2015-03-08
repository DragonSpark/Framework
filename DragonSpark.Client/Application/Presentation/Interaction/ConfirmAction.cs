using System;
using System.Windows;
using System.Windows.Interactivity;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Interaction
{
	public class ConfirmAction : TriggerAction<FrameworkElement>
	{
		public event EventHandler Confirmed = delegate { };

		protected override void Invoke( object parameter )
		{
			var dialogResult = System.Windows.Interactivity.Interaction.GetBehaviors( AssociatedObject ).FirstOrDefaultOfType<IConfirmationDialog>().Transform( y => y.Confirm( parameter ) );
			dialogResult.NotNull( y =>
			{
				y.Value.IsTrue( () => Dispatcher.BeginInvoke( () => Confirmed( this, EventArgs.Empty ) ) );
			} );
		}
	}
}
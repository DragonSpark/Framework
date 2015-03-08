using System;
using System.Windows;

namespace DragonSpark.Application.Presentation.Interaction
{
	public class ShowMessageBox : Expression.Samples.Interactivity.ShowMessageBox
	{
		public event EventHandler Confirmed = delegate { }, Canceled = delegate { };

		protected override void Invoke(object parameter)
		{
			switch ( ResolveResult() )
			{
				case MessageBoxResult.OK:
					Confirmed( this, EventArgs.Empty );
					break;
				case MessageBoxResult.Cancel:
					Canceled( this, EventArgs.Empty );
					break;
			};
		}

		MessageBoxResult ResolveResult()
		{
			var result = string.IsNullOrEmpty( Caption ) ? MessageBox.Show( Message ) : MessageBox.Show( Message, Caption, MessageBoxButton );
			return result;
		}
	}
}
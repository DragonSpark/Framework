using System.Windows;
using System.Windows.Interactivity;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Interaction
{
	public class ConfirmationDialog : Behavior<FrameworkElement>, IConfirmationDialog
	{
		public string DialogTitle
		{
			get { return GetValue( DialogTitleProperty ).To<string>(); }
			set { SetValue( DialogTitleProperty, value ); }
		}	public static readonly DependencyProperty DialogTitleProperty = DependencyProperty.Register( "DialogTitle", typeof(string), typeof(ConfirmationDialog), new PropertyMetadata( "Confirm Action" ) );

		public string Message
		{
			get { return GetValue( MessageProperty ).To<string>(); }
			set { SetValue( MessageProperty, value ); }
		}	public static readonly DependencyProperty MessageProperty = DependencyProperty.Register( "Message", typeof(string), typeof(ConfirmationDialog), new PropertyMetadata( "Are you sure you wish to perform this action?" ) );

		public bool? Confirm( object state )
		{
			var result = MessageBox.Show( Message, DialogTitle, MessageBoxButton.OKCancel ) == MessageBoxResult.OK;
			return result;
		}
	}
}
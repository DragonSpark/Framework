using System.ComponentModel;
using System.Windows;
using System.Windows.Interactivity;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Entity.Interactivity
{
	public class RemovalDialog : Behavior<FrameworkElement>, IRemovalDialog
	{
		public string DialogTitle
		{
			get { return GetValue( DialogTitleProperty ).To<string>(); }
			set { SetValue( DialogTitleProperty, value ); }
		}	public static readonly DependencyProperty DialogTitleProperty = DependencyProperty.Register( "DialogTitle", typeof(string), typeof(RemovalDialog), new PropertyMetadata( "Remove Item" ) );

		public string Message
		{
			get { return GetValue( MessageProperty ).To<string>(); }
			set { SetValue( MessageProperty, value ); }
		}	public static readonly DependencyProperty MessageProperty = DependencyProperty.Register( "Message", typeof(string), typeof(RemovalDialog), new PropertyMetadata( "Are you sure you wish to remove this item?" ) );

		public bool? CanRemove( IRevertibleChangeTracking currentItem )
		{
			var result = MessageBox.Show( Message, DialogTitle, MessageBoxButton.OKCancel ) == MessageBoxResult.OK;
			return result;
		}
	}
}
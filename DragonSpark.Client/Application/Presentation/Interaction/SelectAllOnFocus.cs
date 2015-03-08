using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace DragonSpark.Application.Presentation.Interaction
{
	public class SelectAllOnFocus : Behavior<TextBox>
	{
		protected override void OnAttached()
		{
			AssociatedObject.GotFocus += AssociatedObjectGotFocus;
			base.OnAttached();
		}

		void AssociatedObjectGotFocus( object sender, RoutedEventArgs e )
		{
			AssociatedObject.SelectAll();
		}

		protected override void OnDetaching()
		{
			AssociatedObject.GotFocus -= AssociatedObjectGotFocus;
			base.OnDetaching();
		}
	}
}
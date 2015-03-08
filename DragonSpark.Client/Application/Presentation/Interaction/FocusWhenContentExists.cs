using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Interaction
{
	public class FocusWhenContentExists : Behavior<TextBox>
	{
		protected override void OnAttached()
		{
			AssociatedObject.Loaded += AssociatedObjectLoaded;
			base.OnAttached();
		}

		void AssociatedObjectLoaded( object sender, RoutedEventArgs e )
		{
			string.IsNullOrEmpty( AssociatedObject.Text ).IsFalse( () => AssociatedObject.Focus() );
		}

		protected override void OnDetaching()
		{
			AssociatedObject.Loaded -= AssociatedObjectLoaded;
			base.OnDetaching();
		}
	}
}
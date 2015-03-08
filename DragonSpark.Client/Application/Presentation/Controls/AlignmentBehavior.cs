using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using DragonSpark.Application.Presentation.Extensions;

namespace DragonSpark.Application.Presentation.Controls
{
	public class AlignmentBehavior : Behavior<FrameworkElement>
	{
		protected override void OnAttached()
		{
			AssociatedObject.Loaded += AssociatedObjectLoaded;
		}

		protected override void OnDetaching()
		{
			AssociatedObject.Loaded -= AssociatedObjectLoaded;
		}

		void AssociatedObjectLoaded( object sender, RoutedEventArgs e )
		{
			AssociatedObject.EnsureLoaded( item =>
			{
				var parent = item.GetParentOfType<ContentPresenter>();
				parent.HorizontalAlignment = item.HorizontalAlignment;
				parent.VerticalAlignment = item.VerticalAlignment;
			} );
		}
	}
}
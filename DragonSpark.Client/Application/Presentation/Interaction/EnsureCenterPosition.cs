using System.Windows;
using System.Windows.Interactivity;
using DragonSpark.Application.Presentation.Controls;
using DragonSpark.Application.Presentation.Extensions;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Interaction
{
	public class EnsureCenterPosition : Behavior<DialogChrome>
	{
		protected override void OnAttached()
		{
			AssociatedObject.EnsureLoaded( x => AssociatedObject.Content.As<FrameworkElement>( y =>
			{
			    y.SizeChanged += AssociatedObjectSizeChanged;
			} ) );
			
			base.OnAttached();
		}

		protected override void OnDetaching()
		{
			AssociatedObject.Content.As<FrameworkElement>( y =>
			{
			    y.SizeChanged -= AssociatedObjectSizeChanged;
			} );
			base.OnDetaching();
		}

		void AssociatedObjectSizeChanged( object sender, SizeChangedEventArgs e )
		{
			AssociatedObject.CenterInScreen();
		}
	}
}
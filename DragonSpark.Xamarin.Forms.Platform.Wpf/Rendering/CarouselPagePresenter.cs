using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Xamarin.Forms;
using Page = Xamarin.Forms.Page;

namespace DragonSpark.Client.Windows.Compensations.Rendering
{
	public class CarouselPagePresenter : ContentPresenter
	{
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			var parent = VisualTreeHelper.GetParent( this );
			while ( parent != null && !( parent is CarouselPageItemContainer ) )
			{
				parent = VisualTreeHelper.GetParent( parent );
			}
			CarouselPageItemContainer carouselPageItemContainer = parent as CarouselPageItemContainer;
			if ( carouselPageItemContainer == null )
			{
				throw new Exception( "No parent CarouselPageItemContainer found for carousel page" );
			}
			var element = (FrameworkElement)VisualTreeHelper.GetChild( carouselPageItemContainer, 0 );
			element.SizeChanged += delegate
			{
				if ( element.ActualWidth > 0.0 && element.ActualHeight > 0.0 )
				{
					var page = (Page)this.DataContext;
					( (CarouselPage)page.Parent ).ContainerArea = new Rectangle( 0.0, 0.0, element.ActualWidth, element.ActualHeight );
				}
			};
		}
	}
}

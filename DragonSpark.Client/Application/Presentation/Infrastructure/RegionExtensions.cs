using System.Windows;
using System.Windows.Controls;
using DragonSpark.Application.Presentation.Navigation;
using DragonSpark.Extensions;
using Microsoft.Practices.Prism.Regions;

namespace DragonSpark.Application.Presentation.Infrastructure
{
    public static class RegionExtensions
	{
		public static DependencyObject DetermineContent( this IRegionNavigationService target, object content )
		{
			var element = content.As<FrameworkElement>() ?? new ContentControl { Content = content };
			var result = content.As<Page>() ?? element.Parent ?? target.AsTo<FrameRegionNavigationService, FrameNavigationContainerPage>( x => new FrameNavigationContainerPage ( x ) { Content = element } );
			return result;
		}
	}
}
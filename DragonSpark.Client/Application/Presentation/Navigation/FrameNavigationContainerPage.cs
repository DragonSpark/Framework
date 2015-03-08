using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using DragonSpark.Extensions;
using Microsoft.Practices.Prism.Regions;
using NavigationContext = Microsoft.Practices.Prism.Regions.NavigationContext;

namespace DragonSpark.Application.Presentation.Navigation
{
	class FrameNavigationContainerPage : Page
	{
		readonly FrameRegionNavigationService service;

		public FrameNavigationContainerPage( FrameRegionNavigationService service )
		{
			this.service = service;
		}

		object View
		{
			get
			{
				var result = Content.As<ContentControl>().Transform( x => x.Content, () => Content );
				return result;
			}
		}

		protected IEnumerable<TTargetType> DetermineCandidates<TTargetType>()
		{
			var candidates = new[] { View, View.AsTo<FrameworkElement, object>( x => x.DataContext ) };
			var result = candidates.OfType<TTargetType>();
			return result;
		}
		
		protected override void OnNavigatedTo( NavigationEventArgs e )
		{
			base.OnNavigatedTo( e );
			DetermineCandidates<INavigationAware>().Apply( aware => aware.OnNavigatedTo( CreateNavigationContext( e.Uri ) ) );
		}

		protected override void OnNavigatingFrom( NavigatingCancelEventArgs e )
		{
			DetermineCandidates<IConfirmNavigationRequest>().Apply( request =>
			{
				bool? result = null;
				var ignore = false;

				var context = CreateNavigationContext( e.Uri );
				request.ConfirmNavigationRequest( context, success => ignore.IsFalse( () => result = success ) );

				ignore = true;

				e.Cancel = result.HasValue && !result.GetValueOrDefault();
			} );
		}

		protected override void OnNavigatedFrom( NavigationEventArgs e )
		{
			DetermineCandidates<INavigationAware>().Apply( ina => ina.OnNavigatedFrom( CreateNavigationContext( e.Uri ) ) );
		}

		NavigationContext CreateNavigationContext( Uri uri )
		{
			var result = new NavigationContext( service, service.Transform( x => x.MapUri( uri ) ) ?? uri );
			return result;
		}
	}
}

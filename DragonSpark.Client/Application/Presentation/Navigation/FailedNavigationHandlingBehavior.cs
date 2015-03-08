using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Navigation;
using DragonSpark.Application.Presentation.Infrastructure;
using DragonSpark.Application.Presentation.Launch;
using DragonSpark.Extensions;
using Microsoft.Practices.Prism.Events;

namespace DragonSpark.Application.Presentation.Navigation
{
	public class FailedNavigationHandlingBehavior : RegionBehaviorBase<Frame>
	{
		readonly IEventAggregator aggregator;
		readonly IViewNavigationExceptionHandler handler;
		readonly ExceptionModel model;

		public FailedNavigationHandlingBehavior( IEventAggregator aggregator, IViewNavigationExceptionHandler handler, ExceptionModel model )
		{
			this.aggregator = aggregator;
			this.handler = handler;
			this.model = model;
		}

		protected override Frame AssociatedControl
		{
			set
			{
				AssociatedControl.NotNull( x => x.NavigationFailed -= OnNavigationFailed );

				base.AssociatedControl = value;

				AssociatedControl.NotNull( x => x.NavigationFailed += OnNavigationFailed );
			}
		}

		void OnNavigationFailed( object sender, NavigationFailedEventArgs e )
		{
			e.Handled = e.Uri == null || IsLoading() || Handled( e.Exception, e.Uri );
		}

		bool Handled( Exception exception, Uri location )
		{
			var uri = handler.Handle( exception, location );
			var result = uri != null && AssociatedControl.ContentLoader.CanLoad( uri, location );
			result.IsTrue( () =>
			{
				model.Update( exception, location, uri );
				AssociatedControl.Navigate( uri );
			} );
			return result;
		}

		bool IsLoading()
		{
			return aggregator.GetEvent<ApplicationLaunchEvent>().History.LastOrDefault() < ApplicationLaunchStatus.Loaded;
		}
	}
}
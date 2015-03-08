using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Navigation;
using DragonSpark.Application.Presentation.Infrastructure;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Navigation
{
    public class FrameRegionSyncBehavior : RegionBehaviorBase<Frame>
	{
		bool IsActive { get; set; }

		protected override Frame AssociatedControl
		{
			set
			{
				AssociatedControl.NotNull( x =>
				{
					x.Navigating -= OnNavigating;
					x.NavigationFailed -= OnNavigationStopped;
					x.NavigationStopped -= OnNavigationStopped;
					x.Navigated -= OnFrameNavigated;
				} );

				base.AssociatedControl = value;

				AssociatedControl.NotNull( x =>
				{
					x.Navigating += OnNavigating;
					x.NavigationFailed += OnNavigationStopped;
					x.NavigationStopped += OnNavigationStopped;
					x.Navigated += OnFrameNavigated;
				} );
			}
		}

		void OnNavigating( object s, NavigatingCancelEventArgs a )
		{
			IsActive = false;
		}

		void OnNavigationStopped( object s, EventArgs a )
		{
			IsActive = true;
		}

		protected override void OnAttach()
		{
			base.OnAttach();
			IsActive = true;
			Region.ActiveViews.CollectionChanged += OnActiveViewsChanged;
		}

		void OnFrameNavigated( Object sender, EventArgs e )
		{
			var view = AssociatedControl.Content.AsTo<FrameNavigationContainerPage,object>( x => x.Content.AsTo<ContentControl, object>( y => y.Content ) ?? x.Content );
			view.NotNull( Region.Activate );
			
			IsActive = true;
		}

		void OnActiveViewsChanged( Object sender, EventArgs e )
		{
			IsActive.IsTrue( () => AssociatedControl.Content = Region.ActiveViews.FirstOrDefault().Transform( x => Region.NavigationService.DetermineContent( x ) ) ?? AssociatedControl.Content );
		}
	}
}

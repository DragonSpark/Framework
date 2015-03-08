using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Navigation;
using DragonSpark.Application.Presentation.Extensions;
using DragonSpark.Application.Presentation.Infrastructure;
using DragonSpark.Application.Presentation.Launch;
using DragonSpark.Extensions;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;

namespace DragonSpark.Application.Presentation.Navigation
{
    public class RefreshOnApplicationCompleteBehavior : RegionBehaviorBase<Frame>
	{
		readonly IEventAggregator aggregator;

		public RefreshOnApplicationCompleteBehavior( IEventAggregator aggregator )
		{
			this.aggregator = aggregator;
		}

		Frame ParentFrame { get; set; }

		protected override void OnAttach()
		{
			base.OnAttach();

			AssociatedControl.EnsureLoaded( x =>
			{
				ParentFrame = AssociatedControl.GetParentOfType<Frame>();
				if ( ParentFrame == null )
				{
					aggregator.ExecuteWhenStatusIs( ApplicationLaunchStatus.Complete, () =>
					{
						var stated = System.Windows.Application.Current.Host.NavigationState.NullIfEmpty().Transform( y => new Uri( y, UriKind.Relative ) );
						var uri = stated ?? DetermineDefault();
						uri.NotNull( y =>
						{
							AssociatedControl.Navigated += AssociatedControlOnNavigated;
							AssociatedControl.Navigate( y );
						} );
					} );
				}
				else
				{
					ParentFrame.Navigated += OnParentNavigated;
					EnsureCurrentSource();
				}
			}, false );
		}

		void OnParentNavigated( object sender, NavigationEventArgs e )
		{
			EnsureCurrentSource();
		}

		void EnsureCurrentSource()
		{
			var parent = Region.RegionManager.Regions[ RegionManager.GetRegionName( ParentFrame ) ].GetBehavior<UriMappingContainerBehavior>().Mapper.History.LastOrDefault().Transform( x => x.To );
			var uri = parent.Transform( UriParsingHelper.ParseQuery ).Transform( x => x[ Region.Name ].Transform( y => ParentFrame.CurrentSource ) ) ?? AssociatedControl.CurrentSource ?? DetermineDefault();

			if ( AssociatedControl.CurrentSource != uri )
			{
				AssociatedControl.Navigate( uri );
			}
		}

		Uri DetermineDefault()
		{
			return Region.GetBehavior<UriMappingContainerBehavior>().Transform( y => y.Mapper.UriMappings.FirstOrDefault().Transform( z => z.Uri ) );
		}

		void AssociatedControlOnNavigated( object sender, NavigationEventArgs navigationEventArgs )
		{
			AssociatedControl.Navigated -= AssociatedControlOnNavigated;
			AssociatedControl.Refresh();
		}
	}
}
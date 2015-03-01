using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using DragonSpark.Activation;
using DragonSpark.Extensions;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;

namespace DragonSpark.Client.Windows.Infrastructure
{
	public static class RegionManagerContext
    {
        public static readonly DependencyProperty AssignProperty = DependencyProperty.RegisterAttached( "Assign", typeof(IRegionManager), typeof(RegionManagerContext), new PropertyMetadata( OnAssignPropertyChanged ) );

        static void OnAssignPropertyChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
        {
            ServiceLocation.With<IRegionManagerService>( x => x.Apply( o, e.NewValue.To<IRegionManager>() ) );
        }

        public static IRegionManager GetAssign( FrameworkElement element )
        {
            return (IRegionManager)element.GetValue( AssignProperty );
        }

        public static void SetAssign( FrameworkElement element, IRegionManager value )
        {
            element.SetValue( AssignProperty, value );
        }
    }

	public class RegionManagerService : IRegionManagerService
	{
		readonly IEventAggregator aggregator;
		public event EventHandler Applied = delegate {};

		readonly IList<WeakReference<IRegionManager>> current = new List<WeakReference<IRegionManager>>();

		public RegionManagerService( IEventAggregator aggregator )
		{
			this.aggregator = aggregator;
			aggregator.Subscribe<Launch.ApplicationLaunchEvent, Launch.ApplicationLaunchStatus>( ( evt, p ) =>
			{
				switch ( p )
				{
					case Launch.ApplicationLaunchStatus.Loaded:
						Current.Apply( Broadcast );
						return true;
				}
				return false;
			} );
		}

		void Broadcast( IRegionManager manager )
		{
			Applied( manager, EventArgs.Empty );
		}

		public void Apply( DependencyObject target, IRegionManager regionManager )
		{
			var manager = regionManager.CreateRegionManager();
			RegionManager.SetRegionManager( target, manager );

			if ( IsLoaded() )
			{
				Broadcast( manager );
			}
			current.Add( new WeakReference<IRegionManager>( manager ) );
		}

		bool IsLoaded()
		{
			return aggregator.GetEvent<Launch.ApplicationLaunchEvent>().History.Contains( Launch.ApplicationLaunchStatus.Loaded );
		}

		public IEnumerable<IRegionManager> Current
		{
			get { return IsLoaded() ? current.Targets() : Enumerable.Empty<IRegionManager>(); }
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using DragonSpark.Application.Presentation.Launch;
using DragonSpark.Extensions;
using DragonSpark.IoC;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;

namespace DragonSpark.Application.Presentation.Infrastructure
{
    [Singleton( typeof(IRegionManagerService), Priority = Priority.Lowest )]
    public class RegionManagerService : IRegionManagerService
    {
        readonly IEventAggregator aggregator;
        public event EventHandler Applied = delegate {};

        readonly IList<WeakReference> current = new List<WeakReference>();

        public RegionManagerService( IEventAggregator aggregator )
        {
            this.aggregator = aggregator;
            aggregator.Subscribe<ApplicationLaunchEvent, ApplicationLaunchStatus>( ( evt, p ) =>
            {
                switch ( p )
                {
                    case ApplicationLaunchStatus.Loaded:
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

            /*target.As<FrameworkElement>( x => 
			{
				x.Loaded += XOnLoaded;
				x.Unloaded += OnUnloaded;
			} );*/

            if ( IsLoaded() )
            {
                Broadcast( manager );
            }
            current.Add( new WeakReference( manager ) );
            // current.Add( manager );
	
        }

        /*void XOnLoaded( object sender, RoutedEventArgs routedEventArgs )
		{
			sender.As<FrameworkElement>( x =>
			{
				var manager = RegionManager.GetRegionManager( x );
				current.Add( manager );
			} );
		}

		void OnUnloaded( object sender, RoutedEventArgs e )
		{
			sender.As<FrameworkElement>( x =>
			{
				var manager = RegionManager.GetRegionManager( x );
				// current.Remove( manager );
			} );
		}*/

        bool IsLoaded()
        {
            return aggregator.GetEvent<ApplicationLaunchEvent>().History.Contains( ApplicationLaunchStatus.Loaded );
        }

        public IEnumerable<IRegionManager> Current
        {
            get { return IsLoaded() ? current.Targets<IRegionManager>() : Enumerable.Empty<IRegionManager>(); }
        }
    }
}
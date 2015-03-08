using System;
using System.Windows.Controls;
using System.Windows.Navigation;
using DragonSpark.Extensions;
using Microsoft.Practices.Prism.Regions;
using NavigationContext = Microsoft.Practices.Prism.Regions.NavigationContext;

namespace DragonSpark.Application.Presentation.Navigation
{
	public class FrameRegionNavigationService : IRegionNavigationService
	{
		readonly IRegionNavigationJournal journal;

		public event EventHandler<RegionNavigationEventArgs> Navigated = delegate {};
		public event EventHandler<RegionNavigationEventArgs> Navigating = delegate {};
		public event EventHandler<RegionNavigationFailedEventArgs> NavigationFailed = delegate {};

		public FrameRegionNavigationService()
		{
			journal = new FrameRegionNavigationJournal( this );
		}

		public IRegion Region { get; set; }

		public Frame Frame { get; set; }

		public IRegionNavigationJournal Journal
		{
			get { return journal; }
		}

		public void RequestNavigate( Uri source, Action<NavigationResult> navigationCallback )
		{
			new FrameNavigationRequestAdapter( this, source, navigationCallback ).RequestNavigate();
		}

		void RaiseNavigated( NavigationContext navigationContext )
		{
			Navigated( this, new RegionNavigationEventArgs( navigationContext ) );
		}

		void RaiseNavigating( NavigationContext navigationContext )
		{
			Navigating( this, new RegionNavigationEventArgs( navigationContext ) );
		}

		void RaiseNavigationFailed( NavigationContext navigationContext, Exception error )
		{
			NavigationFailed( this, new RegionNavigationFailedEventArgs( navigationContext, error ) );
		}

		internal Uri MapUri( Uri uri )
		{
			var result = Frame.Transform( x => x.UriMapper.Transform( y => y.MapUri( uri ), () => uri )  );
			return result;
		}

		class FrameNavigationRequestAdapter
		{
			readonly FrameRegionNavigationService service;
			readonly Uri source, mappedSource;
			readonly Action<NavigationResult> navigationCallback;
			readonly Frame frame;

			public FrameNavigationRequestAdapter( FrameRegionNavigationService service, Uri source, Action<NavigationResult> navigationCallback )
			{
				this.service = service;
				mappedSource = source;
				this.source = service.MapUri( source );
				this.navigationCallback = navigationCallback;
				frame = service.Frame;

				frame.Navigated += OnFrameNavigated;
				frame.NavigationFailed += OnFrameNavigationFailed;
				frame.NavigationStopped += OnFrameNavigationStopped;
			}

			void UnhookFromFrame()
			{
				frame.Navigated -= OnFrameNavigated;
				frame.NavigationFailed -= OnFrameNavigationFailed;
				frame.NavigationStopped -= OnFrameNavigationStopped;
			}

			void OnFrameNavigated( Object sender, NavigationEventArgs args )
			{
				UnhookFromFrame();
				var context = new NavigationContext( service, source );
				service.RaiseNavigating( context );
				navigationCallback( new NavigationResult( context, true ) );
				service.RaiseNavigated( context );
			}

			void OnFrameNavigationFailed( Object sender, NavigationFailedEventArgs args )
			{
				UnhookFromFrame();
				args.Handled = true;
				var navigationContext = new NavigationContext( service, source );
				var navigationResult = new NavigationResult( navigationContext, args.Exception );
				navigationCallback( navigationResult );
				service.RaiseNavigationFailed( navigationContext, args.Exception );
			}

			void OnFrameNavigationStopped( Object sender, NavigationEventArgs args )
			{
				UnhookFromFrame();
			}

			internal void RequestNavigate()
			{
				frame.Navigate( mappedSource );
			}
		}
	}
}

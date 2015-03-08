using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Navigation;
using DragonSpark.Application.Presentation.ComponentModel;
using DragonSpark.Application.Presentation.Extensions;
using DragonSpark.Application.Presentation.Infrastructure;
using DragonSpark.Application.Presentation.Launch;
using DragonSpark.Extensions;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using NavigationContext = Microsoft.Practices.Prism.Regions.NavigationContext;

namespace DragonSpark.Application.Presentation.Navigation
{
	public class FrameContentLoader : ViewObject, INavigationContentLoader
	{
		readonly IEventAggregator aggregator;
		readonly IRegionNavigationContentLoader loader;
		readonly IEnumerable<IViewValidator> validators;
		readonly IRegion region;

		public FrameContentLoader( IEventAggregator aggregator, IRegionNavigationContentLoader loader, IEnumerable<IViewValidator> validators, IRegion region )
		{
			this.aggregator = aggregator;
			this.loader = loader;
			this.validators = validators;
			this.region = region;
		}

		bool INavigationContentLoader.CanLoad( Uri targetUri, Uri currentUri )
		{
			var result = IsReady() && region.GetBehavior<UriMappingContainerBehavior>().Contains( targetUri );
			return result;
		}

		internal bool IsReady()
		{
			return aggregator.GetEvent<ApplicationLaunchEvent>().History.Contains( ApplicationLaunchStatus.Complete );
		}

		IAsyncResult INavigationContentLoader.BeginLoad( Uri targetUri, Uri currentUri, AsyncCallback userCallback, object asyncState )
		{
			var result = new LoadAsyncResult( targetUri, currentUri, asyncState );
			userCallback.NotNull( x => x( result ) );
			return result;
		}

		LoadResult INavigationContentLoader.EndLoad( IAsyncResult asyncResult )
		{
			var result = asyncResult.AsTo<LoadAsyncResult, LoadResult>( DetermineResult );
			return result;
		}

		void INavigationContentLoader.CancelLoad( IAsyncResult asyncResult )
		{}
		
		LoadResult DetermineResult( LoadAsyncResult source )
		{
			var same = source.CurrentUri == source.TargetUri;
			var result = same ? region.ActiveViews.FirstOrDefault().Transform( x => new LoadResult( x ), () => Load( source ) ) : Load( source );
			return result;
		}

		LoadResult Load( LoadAsyncResult source )
		{
			var view = loader.LoadContent( region, new NavigationContext( null, source.TargetUri ) );
			var context = new ViewValidationContext( region, source.TargetUri, view );

			validators.Apply( x => x.Validate( context ) );

			var result = new LoadResult( context.Region.NavigationService.DetermineContent( context.Content ) );
			return result;
		}

		class LoadAsyncResult : IAsyncResult
		{
			public LoadAsyncResult( Uri targetUri, Uri currentUri, object asyncState )
			{
				TargetUri = targetUri;
				CurrentUri = currentUri;
				AsyncState = asyncState;
			}

			public object AsyncState { get; private set; }
			public Uri TargetUri { get; private set; }
			public Uri CurrentUri { get; private set; }

			WaitHandle IAsyncResult.AsyncWaitHandle
			{
				get { return null; }
			}

			Boolean IAsyncResult.CompletedSynchronously
			{
				get { return true; }
			}

			Boolean IAsyncResult.IsCompleted
			{
				get { return true; }
			}
		}
	}
}

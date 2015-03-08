using System;
using System.Linq;
using System.Windows.Controls;
using DragonSpark.Application.Presentation.Infrastructure;
using DragonSpark.Extensions;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.Regions.Behaviors;
using Microsoft.Practices.Unity.Utility;
using Activator = DragonSpark.Runtime.Activator;

namespace DragonSpark.Application.Presentation.Navigation
{
	public class FrameRegionAdapter : RegionAdapterBase<Frame>
	{
		static readonly Type[] BehaviorTypes = typeof(FrameRegionAdapter).Assembly.GetExportedTypes().Where( typeof(RegionBehaviorBase<Frame>).IsAssignableFrom ).ToArray();

		public FrameRegionAdapter( IRegionBehaviorFactory regionBehaviorFactory ) : base( regionBehaviorFactory )
		{}

		protected override void Adapt( IRegion region, Frame regionTarget )
		{
			Guard.ArgumentNotNull( regionTarget, "regionTarget" );
			region.NavigationService = new FrameRegionNavigationService { Frame = regionTarget, Region = region };

			// regionTarget.Source = regionTarget.Source ?? new Uri( string.Empty, UriKind.RelativeOrAbsolute );
			regionTarget.ContentLoader = Activator.Create<FrameContentLoader>( region );
			regionTarget.Content = new UserControl();
		}

		/// <summary>
		/// Attaches the behaviors.
		/// </summary>
		/// <param name="region">The region.</param>
		/// <param name="regionTarget">The region target.</param>
		protected override void AttachBehaviors( IRegion region, Frame regionTarget )
		{
			Guard.ArgumentNotNull( region, "region" );

			base.AttachBehaviors( region, regionTarget );

			BehaviorTypes.Apply( x => region.Behaviors.ContainsKey( x.Name ).IsFalse( () => Activator.CreateInstance<RegionBehavior>( x ).NotNull( z =>
			{
				z.As<IHostAwareRegionBehavior>( a => a.HostControl = regionTarget );
				region.Behaviors.Add( x.Name, z );
			} ) ) );
		}

		/// <summary>
		/// Creates a new instance of <see cref="SingleActiveRegion"/>.
		/// </summary>
		/// <returns>A new instance of <see cref="SingleActiveRegion"/>.</returns>
		protected override IRegion CreateRegion()
		{
			return new SingleActiveRegion();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Markup;
using DragonSpark.Extensions;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace DragonSpark.Application.Presentation.Infrastructure
{
	[ContentProperty( "Actions" )]
	public class RegionViewRegistration : ViewRegistration
	{
		public string RegionName { get; set; }

		[TypeConverter( typeof(StringArrayConverter) )]
		public string[] SupportedProfiles { get; set; }
		
		public Collection<RegionViewRegistrationAction> Actions
		{
			get { return actions; }
		}	readonly Collection<RegionViewRegistrationAction> actions = new Collection<RegionViewRegistrationAction>();

		IUnityContainer Container { get; set; }

		Type ViewType { get; set; }	

		IViewProfileService ViewProfileService { get; set; }

		protected virtual IRegion Determine( IEnumerable<IRegion> regions )
		{
			var result = SupportedProfiles == null || SupportedProfiles.Contains( ViewProfileService.SelectedProfile.Identifier ) ? regions.FirstOrDefault( x => x.Name == RegionName ) : null;
			return result;
		}

		protected override void Process( IUnityContainer container, Type viewType )
		{
			Container = container;
			ViewType = viewType;
			ViewProfileService = container.Resolve<IViewProfileService>();

			var service = container.Resolve<IRegionManagerService>();
			service.Applied += ( s, a ) => s.As<IRegionManager>( Apply );
			service.Current.Apply( Apply );
		}

		void Apply( IRegionManager x )
		{
			Determine( x.Regions ).NotNull( Register );

			x.Regions.CollectionChanged += ( o, args ) =>
			{
				switch ( args.Action )
				{
					case NotifyCollectionChangedAction.Add:
						var regions = args.NewItems.Cast<IRegion>();
						Determine( regions ).NotNull( Register );
						break;
				}
			};
		}

		void Register( IRegion region )
		{
			Actions.Apply( x => x.Process( Container, region, ViewType ) );
		}
	}
}
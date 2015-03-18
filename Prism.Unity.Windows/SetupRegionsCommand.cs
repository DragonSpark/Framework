using Microsoft.Practices.ServiceLocation;
using Prism.Logging;
using Prism.Properties;
using Prism.Regions;
using Prism.Regions.Behaviors;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Prism.Unity.Windows
{
	public class SetupRegionsCommand : SetupCommand
	{
		protected override void Execute( SetupContext context )
		{
			context.Logger.Log(Resources.ConfiguringRegionAdapters, Category.Debug, Priority.Low);
			this.ConfigureRegionAdapterMappings();

			context.Logger.Log(Resources.ConfiguringDefaultRegionBehaviors, Category.Debug, Priority.Low);
			this.ConfigureDefaultRegionBehaviors();
		}

		protected virtual RegionAdapterMappings ConfigureRegionAdapterMappings()
		{
			RegionAdapterMappings regionAdapterMappings = ServiceLocator.Current.GetInstance<RegionAdapterMappings>();
			if (regionAdapterMappings != null)
			{
				regionAdapterMappings.RegisterMapping(typeof(Selector), ServiceLocator.Current.GetInstance<SelectorRegionAdapter>());
				regionAdapterMappings.RegisterMapping(typeof(ItemsControl), ServiceLocator.Current.GetInstance<ItemsControlRegionAdapter>());
				regionAdapterMappings.RegisterMapping(typeof(ContentControl), ServiceLocator.Current.GetInstance<ContentControlRegionAdapter>());
			}

			return regionAdapterMappings;
		}

		/// <summary>
		/// Configures the <see cref="IRegionBehaviorFactory"/>. 
		/// This will be the list of default behaviors that will be added to a region. 
		/// </summary>
		protected virtual IRegionBehaviorFactory ConfigureDefaultRegionBehaviors()
		{
			var defaultRegionBehaviorTypesDictionary = ServiceLocator.Current.GetInstance<IRegionBehaviorFactory>();

			if (defaultRegionBehaviorTypesDictionary != null)
			{
				defaultRegionBehaviorTypesDictionary.AddIfMissing(BindRegionContextToDependencyObjectBehavior.BehaviorKey,
					typeof(BindRegionContextToDependencyObjectBehavior));

				defaultRegionBehaviorTypesDictionary.AddIfMissing(RegionActiveAwareBehavior.BehaviorKey,
					typeof(RegionActiveAwareBehavior));

				defaultRegionBehaviorTypesDictionary.AddIfMissing(SyncRegionContextWithHostBehavior.BehaviorKey,
					typeof(SyncRegionContextWithHostBehavior));

				defaultRegionBehaviorTypesDictionary.AddIfMissing(RegionManagerRegistrationBehavior.BehaviorKey,
					typeof(RegionManagerRegistrationBehavior));

				defaultRegionBehaviorTypesDictionary.AddIfMissing(RegionMemberLifetimeBehavior.BehaviorKey,
					typeof(RegionMemberLifetimeBehavior));

				defaultRegionBehaviorTypesDictionary.AddIfMissing(ClearChildViewsRegionBehavior.BehaviorKey,
					typeof(ClearChildViewsRegionBehavior));

				defaultRegionBehaviorTypesDictionary.AddIfMissing(AutoPopulateRegionBehavior.BehaviorKey,
					typeof(AutoPopulateRegionBehavior));
			}

			return defaultRegionBehaviorTypesDictionary;
		}
	}
}
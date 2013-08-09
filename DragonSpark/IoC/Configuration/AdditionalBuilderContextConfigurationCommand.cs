using System.Collections.ObjectModel;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

namespace DragonSpark.IoC.Configuration
{
	public class AdditionalBuilderContextConfigurationCommand : IContainerConfigurationCommand
	{
		public void Configure( IUnityContainer container )
		{
			var configuration = container.Configure<IConfigurationExtensionConfiguration>();
			if ( configuration != null )
			{
				// Add any additional build strategies:
				using ( var child = container.CreateChildContainer() )
				{
					foreach ( var strategy in Strategies )
					{
						configuration.AddStrategy( (IBuilderStrategy)child.Resolve( strategy.StrategyType ), strategy.Stage );
					}
				}

				// Set any default policies:
				foreach ( var element in DefaultPolicies )
				{
					configuration.AddDefaultPolicy( element.BuildType, element.CreatePolicy( container ) );
				}
			}
		}

		public ObservableCollection<PolicyReference> DefaultPolicies
		{
			get { return defaultPolicies ?? ( defaultPolicies = new ObservableCollection<PolicyReference>() ); }
		}	ObservableCollection<PolicyReference> defaultPolicies;

		public ObservableCollection<StrategyReference> Strategies
		{
			get { return strategies ?? ( strategies = new ObservableCollection<StrategyReference>() ); }
		}	ObservableCollection<StrategyReference> strategies;
	}
}
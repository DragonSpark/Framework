using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;
using System;

namespace DragonSpark.IoC.Configuration
{
	public class ConfigurationExtension : UnityContainerExtension, IConfigurationExtensionConfiguration
	{
		readonly static ClearStrategyExtension Extension = new ClearStrategyExtension();

		protected override void Initialize()
		{}

		public void AddStrategy( IBuilderStrategy strategy, UnityBuildStage stage )
		{
			Context.Container.AddExtension( Extension ); // HACK: Weakkkkkk SAUUUUUUUUUCEEEEEEEE!!!
			Context.Strategies.Add( strategy, stage );
		}

		public void AddDefaultPolicy( Type policyInterface, IBuilderPolicy policy )
		{
			Context.Policies.SetDefault( policyInterface, policy );
		}
	}
}
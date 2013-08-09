using System;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;

namespace DragonSpark.IoC.Configuration
{
	interface IConfigurationExtensionConfiguration : IUnityContainerExtensionConfigurator
	{
		void AddStrategy( IBuilderStrategy strategy, UnityBuildStage stage );
		void AddDefaultPolicy( Type policyInterface, IBuilderPolicy policy );
	}
}
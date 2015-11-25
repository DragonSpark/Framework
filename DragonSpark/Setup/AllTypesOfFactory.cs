using DragonSpark.Activation;
using DragonSpark.Runtime;
using Microsoft.Practices.Unity;
using System;
using System.Linq;

namespace DragonSpark.Setup
{
	[LifetimeManager( typeof(ContainerControlledLifetimeManager) )]
	public class AllTypesOfFactory : FactoryBase<Type, Array>
	{
		readonly IAssemblyProvider provider;
		readonly IActivator activator;

		public AllTypesOfFactory( IAssemblyProvider provider, IActivator activator )
		{
			this.provider = provider;
			this.activator = activator;
		}

		protected override Array CreateFrom( Type parameter )
		{
			// var type = parameter ?? resultType.Extend().GetInnerType() ?? resultType;
			var types = provider.GetAssemblies().SelectMany( assembly => assembly.ExportedTypes );
			var items = activator.ActivateMany( parameter, types ).ToArray();
			var result = Array.CreateInstance( parameter, items.Length );
			Array.Copy( items, result, items.Length );
			return result;
		}
	}
}
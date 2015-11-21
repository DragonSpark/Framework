using DragonSpark.Activation;
using DragonSpark.Extensions;
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

		protected override Array CreateFrom( Type resultType, Type parameter )
		{
			var type = parameter ?? resultType.GetInnerType() ?? resultType;
			var items = provider.GetAssemblies().SelectMany( assembly => assembly.ExportedTypes.Where( type.CanActivate ) ).Select( activator.Activate<object> ).ToArray();
			var result = Array.CreateInstance( type, items.Length );
			Array.Copy( items, result, items.Length );
			return result;
		}
	}
}
using System;
using System.Linq;
using DragonSpark.Activation;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using Microsoft.Practices.Unity;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Setup
{
	[LifetimeManager( typeof(ContainerControlledLifetimeManager) )]
	public class AllTypesOfFactory : FactoryBase<Type, Array>
	{
		readonly IAssemblyProvider provider;

		public AllTypesOfFactory( IAssemblyProvider provider )
		{
			this.provider = provider;
		}

		protected override Array CreateFrom( Type resultType, Type parameter )
		{
			var type = parameter ?? resultType.GetInnerType() ?? resultType;
			var items = provider.GetAssemblies().SelectMany( assembly => assembly.ExportedTypes.Where( t => TypeExtensions.CanActivate( t, type ) ) ).Select( Activator.CreateInstance<object> ).ToArray();
			var result = Array.CreateInstance( type, items.Length );
			Array.Copy( items, result, items.Length );
			return result;
		}
	}
}
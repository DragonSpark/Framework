using System;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

namespace DragonSpark.IoC
{
	public class AllAppDomainTypesOfParameter : ResolveAllOfTypeParameter
	{
		readonly Type[] typesToExclude;

		public AllAppDomainTypesOfParameter( Type elementType, params Type[] typesToExclude ) : base( elementType )
		{
			this.typesToExclude = typesToExclude;
		}

		public override IDependencyResolverPolicy GetResolverPolicy(Type typeToBuild)
		{
			return this;
		}

		/*public object Resolve( IBuilderContext context )
		{
			var result = ;
			return result;
		}*/

		protected override object[] ResolveItems(IUnityContainer container)
		{
			var result = new AppDomainTypeResolver( container ).Resolve( ElementType, typesToExclude );
			return result;
		}
	}
}
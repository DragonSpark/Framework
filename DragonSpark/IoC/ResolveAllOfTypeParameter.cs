using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DragonSpark.Extensions;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Activator = DragonSpark.Runtime.Activator;

namespace DragonSpark.IoC
{
	public class ResolveAllOfTypeParameter : TypedInjectionValue, IDependencyResolverPolicy
	{
		readonly Type elementType;

		public ResolveAllOfTypeParameter( Type elementType ) : base( typeof(IEnumerable<>).MakeGenericType( elementType ) )
		{
			this.elementType = elementType;
		}

		public Type ElementType
		{
			get { return elementType; }
		}

		public override IDependencyResolverPolicy GetResolverPolicy(Type typeToBuild)
		{
			return this;
		}

		public object Resolve( IBuilderContext context )
		{
			var container = context.NewBuildUp<IUnityContainer>();
			var result = Activator.CreateInstance<IList>( typeof(List<>).MakeGeneric( elementType ) );
			var items = ResolveItems( container );
			items.Apply( x => result.Add( x ) );
			return result;
		}

		protected virtual object[] ResolveItems( IUnityContainer container )
		{
			return container.ResolveAllRegistered( elementType ).ToArray();
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity;

namespace DragonSpark.IoC
{
	public class AppDomainTypeResolver
	{
		static readonly MethodInfo ResolveInternalMethod = typeof(AppDomainTypeResolver).GetMethod( "ResolveInternal", DragonSparkBindingOptions.AllProperties );

		readonly IUnityContainer container;

		public AppDomainTypeResolver( IUnityContainer container )
		{
			this.container = container;
		}

		public object[] Resolve( Type elementType, params Type[] typesToExclude )
		{
			var result = ResolveInternalMethod.MakeGenericMethod( elementType ).Invoke( null, new object[] { container, typesToExclude } );
			return (object[])result;
		}

		static TItem[] ResolveInternal<TItem>( IUnityContainer container, IEnumerable<Type> typesToExclude )
		{
			var result = AppDomain.CurrentDomain.GetAssemblies().SelectMany( x => x.GetTypes() ).Where( x => !x.IsAbstract && !x.IsInterface && typeof(TItem).IsAssignableFrom( x ) ).Except( typesToExclude ).Select( x => container.Resolve( x ) ).Cast<TItem>().ToArray();
			return result;
		}
	}
}
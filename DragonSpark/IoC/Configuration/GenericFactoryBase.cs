using System;
using System.Collections.Generic;
using System.Reflection;
using DragonSpark.Extensions;
using Microsoft.Practices.Unity;

namespace DragonSpark.IoC.Configuration
{
	public abstract class GenericEnumerableFactoryBase : FactoryBase
	{
		static readonly MethodInfo CreateItemMethod = typeof(GenericEnumerableFactoryBase).GetMethod( "CreateItem", DragonSparkBindingOptions.AllProperties );

		object CreateItem<TItem>( IUnityContainer container, string buildName )
		{
			var result = Create<TItem>( container, buildName );
			return result;
		}

		protected abstract IEnumerable<TItem> Create<TItem>( IUnityContainer container, string buildName );

		protected override sealed object Create( IUnityContainer container, Type type, string buildName )
		{
			var result = CreateItemMethod.MakeGenericMethod( type.GetCollectionElementType() ).Invoke( this, new object[] { container, buildName } );
			return result;
		}
	}
}
using System;
using System.Reflection;
using Microsoft.Practices.Unity;

namespace DragonSpark.IoC.Configuration
{
	public abstract class GenericItemFactoryBase : FactoryBase
	{
		static readonly MethodInfo CreateItemMethod = typeof(GenericItemFactoryBase).GetMethod( "CreateItem", DragonSparkBindingOptions.AllProperties );

		object CreateItem<TItem>( IUnityContainer container, string buildName )
		{
			var result = Create<TItem>( container, buildName );
			return result;
		}

		protected abstract TItem Create<TItem>( IUnityContainer container, string buildName );

		protected override sealed object Create( IUnityContainer container, Type type, string buildName )
		{
			var result = CreateItemMethod.MakeGenericMethod( type ).Invoke( this, new object[] { container, buildName } );
			return result;
		}
	}
}
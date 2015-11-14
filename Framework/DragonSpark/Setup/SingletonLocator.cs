using DragonSpark.Extensions;
using System;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Setup
{
	public class SingletonLocator : ISingletonLocator
	{
		public static SingletonLocator Instance { get; } = new SingletonLocator();

		readonly string property;

		public SingletonLocator( string property = "Instance" )
		{
			this.property = property;
		}

		public object Locate( Type type )
		{
			var property = type.GetTypeInfo().DeclaredProperties.FirstOrDefault( info => /*info.PropertyType == type &&*/ info.GetMethod.IsStatic && ( info.Name == this.property || info.IsDecoratedWith<SingletonAttribute>() ) );
			var result = property.Transform( info => info.GetValue( null ) );
			return result;
		}
	}
}
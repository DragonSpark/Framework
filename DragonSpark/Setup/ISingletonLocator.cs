using System;
using System.Linq;
using System.Reflection;
using DragonSpark.Extensions;

namespace DragonSpark.Setup
{
	public interface ISingletonLocator
	{
		object Locate( Type type );
	}

	public class SingletonLocator : ISingletonLocator
	{
		public static SingletonLocator Instance { get; } = new SingletonLocator();

		public object Locate( Type type )
		{
			var property = type.GetTypeInfo().DeclaredProperties.FirstOrDefault( info => info.PropertyType == type && info.GetMethod.IsStatic && ( info.Name == "Instance" || info.IsDecoratedWith<SingletonAttribute>() ) );
			var result = property.Transform( info => info.GetValue( null ) );
			return result;
		}
	}

	[AttributeUsage( AttributeTargets.Property )]
	public class SingletonAttribute : Attribute
	{}
}
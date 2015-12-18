using DragonSpark.Extensions;
using System;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Activation.IoC
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
			var mapped = type.Adapt().DetermineImplementor() ?? type.GetTypeInfo();
			var declared = mapped.DeclaredProperties.FirstOrDefault( info => info.GetMethod.IsStatic && !info.GetMethod.ContainsGenericParameters && ( info.Name == property || info.IsDecoratedWith<SingletonAttribute>() ) );
			var result = declared.With( info => info.GetValue( null ) );
			return result;
		}
	}
}
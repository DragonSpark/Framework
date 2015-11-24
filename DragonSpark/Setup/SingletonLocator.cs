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
			var typeInfo = type.GetTypeInfo();
			var mapped = typeInfo.IsInterface ? DetermineType( typeInfo ) : typeInfo;
			var declared = mapped.DeclaredProperties.FirstOrDefault( info => info.GetMethod.IsStatic && ( info.Name == property || info.IsDecoratedWith<SingletonAttribute>() ) );
			var result = declared.Transform( info => info.GetValue( null ) );
			return result;
		}

		static TypeInfo DetermineType( TypeInfo type )
		{
			try
			{
				var name = string.Concat( type.Namespace, ".asdf", type.Name.Substring(1) );
				var result = type.Assembly.GetType( name ).GetTypeInfo();
				return result;
			}
			catch ( Exception e )
			{
				return type;
			}
		}
	}
}
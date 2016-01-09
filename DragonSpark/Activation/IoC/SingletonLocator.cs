using DragonSpark.Extensions;
using PostSharp.Patterns.Contracts;
using System;
using System.Linq;
using System.Reflection;
using DragonSpark.TypeSystem;

namespace DragonSpark.Activation.IoC
{
	public class SingletonLocator : ISingletonLocator
	{
		public static SingletonLocator Instance { get; } = new SingletonLocator();

		readonly IAttributeProvider provider;
		readonly string property;

		public SingletonLocator( string property = "Instance" ) : this( AttributeProvider.Instance, property ) {}

		public SingletonLocator( [Required]IAttributeProvider provider, string property = "Instance" )
		{
			this.provider = provider;
			this.property = property;
		}

		public object Locate( Type type )
		{
			var mapped = type.Adapt().DetermineImplementor() ?? type.GetTypeInfo();
			var declared = mapped.DeclaredProperties.FirstOrDefault( info => info.GetMethod.IsStatic && !info.GetMethod.ContainsGenericParameters && ( info.Name == property || provider.IsDecoratedWith<SingletonAttribute>( info ) ) );
			var result = declared.With( info => info.GetValue( null ) );
			return result;
		}
	}
}
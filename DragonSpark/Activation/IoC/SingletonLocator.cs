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
		public const string Instance = "Instance";

		// public static SingletonLocator Instance { get; } = new SingletonLocator();

		readonly BuildableTypeFromConventionLocator locator;
		readonly string property;

		// public SingletonLocator( string property = nameof(Instance) ) : this( BuildableTypeFromConventionLocator.Instance, property ) {}

		public SingletonLocator( string property = "Instance" ) : this( new BuildableTypeFromConventionLocator( Assemblies.GetCurrent() ), property ) {}

		public SingletonLocator( [Required]BuildableTypeFromConventionLocator locator, string property = Instance )
		{
			this.locator = locator;
			this.property = property;
		}

		public object Locate( Type type )
		{
			var mapped = locator.Create( type )?.GetTypeInfo() ?? type.GetTypeInfo();
			var declared = mapped.DeclaredProperties.FirstOrDefault( info => info.GetMethod.IsStatic && !info.GetMethod.ContainsGenericParameters && ( info.Name == property || info.Has<SingletonAttribute>() ) );
			var result = declared.With( info => info.GetValue( null ) );
			return result;
		}
	}
}
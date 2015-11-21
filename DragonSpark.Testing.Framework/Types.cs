using DragonSpark.Activation;
using DragonSpark.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Testing.Framework
{
	[AttributeUsage( AttributeTargets.Class )]
	public class TypesAttribute : Attribute { }

	[Types]
	public abstract class Types
	{}

	public class Types<T> : Types { }

	public class Types<T, U> : Types { }

	public class Types<T, U, V> : Types { }

	public class Types<T, U, V, W> : Types { }

	public class Types<T, U, V, W, X> : Types { }

	public class Types<T, U, V, W, X, Y> : Types { }

	public class Types<T, U, V, W, X, Y, Z> : Types { }

	public class TypesFactory<T> : FactoryBase<Type[], IEnumerable<T>>
	{
		readonly IActivator activator;
		public static TypesFactory<T> Instance { get; } = new TypesFactory<T>();

		public TypesFactory() : this( SystemActivator.Instance )
		{}

		public TypesFactory( IActivator activator )
		{
			this.activator = activator;
		}

		protected override IEnumerable<T> CreateFrom( Type resultType, Type[] parameter )
		{
			var types = DetermineTypes( parameter );
			var result = activator.ActivateMany<T>( types );
			return result;
		}

		static Type[] DetermineTypes( IEnumerable<Type> parameter )
		{
			var result = parameter.SelectMany( Select ).Distinct().ToArray();
			return result;
		}

		static IEnumerable<Type> Expand( Type type )
		{
			var hierarchy = type.GetHierarchy( false );
			var result = hierarchy.SelectMany( t => t.IsGenericType ? t.GenericTypeArguments.SelectMany( Select	) : t.Append() );
			return result;
		}

		static IEnumerable<Type> Select( Type type )
		{
			var result = type.IsDecoratedWith<TypesAttribute>() ? Expand( type ) : type.Append();
			return result;
		}
	}
}
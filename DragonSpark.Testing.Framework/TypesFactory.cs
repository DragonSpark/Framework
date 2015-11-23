using System;
using System.Collections.Generic;
using System.Linq;
using DragonSpark.Activation;
using DragonSpark.Extensions;

namespace DragonSpark.Testing.Framework
{
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
			var hierarchy = type.Extend().GetHierarchy( false );
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
using DragonSpark.Activation;
using DragonSpark.Sources.Parameterized;
using DragonSpark.TypeSystem;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Aspects
{
	public class AdapterConstructorSource<T> : ParameterizedSourceBase<Type, Func<object, T>>
	{
		readonly static Func<Type, Type, Func<object, T>> Factory = ParameterConstructor<object, T>.Make;

		readonly Type parameterType;
		readonly Type implementedType;
		readonly Type adapterType;
		readonly Func<Type, Type, Func<object, T>> factory;

		public AdapterConstructorSource( Type implementedType, Type adapterType ) : this( implementedType, implementedType, adapterType ) {}

		public AdapterConstructorSource( Type parameterType, Type implementedType, Type adapterType ) : this( parameterType, implementedType, adapterType, Factory ) {}

		protected AdapterConstructorSource( Type parameterType, Type implementedType, Type adapterType, Func<Type, Type, Func<object, T>> factory )
		{
			this.parameterType = parameterType;
			this.implementedType = implementedType;
			this.adapterType = adapterType;
			this.factory = factory;
		}

		public override Func<object, T> Get( Type parameter )
		{
			var adapter = parameter.Adapt();
			var instanceParameterType = adapter.GetImplementations( parameterType ).OrderByDescending( type => type, Comparer.Default ).First();
			var instanceImplementedType = parameterType == implementedType ? instanceParameterType : adapter.GetImplementations( implementedType ).OrderByDescending( type => type, Comparer.Default ).First();
			var resultType = adapterType.MakeGenericType( instanceImplementedType.GenericTypeArguments );
			var result = factory( instanceParameterType, resultType );
			return result;
		}

		sealed class Comparer : IComparer<Type>
		{
			public static Comparer Default { get; } = new Comparer();
			Comparer() {}

			public int Compare( Type x, Type y ) => 
				x.GenericTypeArguments.Count( type => type != typeof(object) ).CompareTo( y.GenericTypeArguments.Count( type => type != typeof(object) ) );
		}
	}
}
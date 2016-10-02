using DragonSpark.Sources.Parameterized;
using System;

namespace DragonSpark.Aspects
{
	public abstract class ConstructedSourceBase<T> : ParameterizedSourceBase<Type, T>
	{
		readonly Func<Type, Func<object, T>> constructorSource;
		readonly Func<Type, object> argumentSource;

		protected ConstructedSourceBase( Func<Type, Func<object, T>> constructorSource ) : this( constructorSource, Defaults.ArgumentSource ) {}

		protected ConstructedSourceBase( Func<Type, Func<object, T>> constructorSource, Func<Type, object> argumentSource )
		{
			this.constructorSource = constructorSource;
			this.argumentSource = argumentSource;
		}

		public override T Get( Type parameter )
		{
			var constructor = constructorSource( parameter );
			var argument = argumentSource( parameter );
			var result = constructor( argument );
			return result;
		}
	}
}
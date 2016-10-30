using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using System;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Aspects
{
	public abstract class ConstructedSourceBase<T> : ParameterizedSourceBase<Type, T>
	{
		readonly Func<Type, Func<object, T>> constructorSource;
		readonly Func<Type, object> argumentSource;

		protected ConstructedSourceBase( Func<Type, Func<object, T>> constructorSource ) : this( constructorSource, Activator.Default.GetService ) {}

		protected ConstructedSourceBase( Func<Type, Func<object, T>> constructorSource, Func<Type, object> argumentSource )
		{
			this.constructorSource = constructorSource;
			this.argumentSource = argumentSource;
		}

		public override T Get( Type parameter )
		{
			var constructor = constructorSource( parameter );
			var argument = argumentSource( parameter ).Account( parameter );
			var result = constructor( argument );
			return result;
		}
	}
}
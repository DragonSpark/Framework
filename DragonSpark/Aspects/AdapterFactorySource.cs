using DragonSpark.Sources.Parameterized;
using System;

namespace DragonSpark.Aspects
{
	public class AdapterFactorySource : AdapterFactorySource<object>
	{
		public AdapterFactorySource( Type implementedType, Type adapterType ) : base( implementedType, adapterType ) {}
		public AdapterFactorySource( Type parameterType, Type implementedType, Type adapterType ) : base( parameterType, implementedType, adapterType ) {}
	}

	public class AdapterFactorySource<T> : ParameterizedSourceBase<T>
	{
		readonly Func<Type, Func<object, T>> factorySource;

		public AdapterFactorySource( Type implementedType, Type adapterType ) : this( implementedType, implementedType, adapterType ) {}
		public AdapterFactorySource( Type parameterType, Type implementedType, Type adapterType ) : this( new AdapterConstructorSource<T>( parameterType, implementedType, adapterType ).ToCache().ToDelegate() ) {}

		AdapterFactorySource( Func<Type, Func<object, T>> factorySource )
		{
			this.factorySource = factorySource;
		}

		public override T Get( object parameter ) => factorySource( parameter.GetType() ).Invoke( parameter );
	}
}
using DragonSpark.Activation;
using DragonSpark.Expressions;
using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.TypeSystem.Generics;
using System;
using System.Collections.Immutable;
using System.Linq;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Sources.Delegates
{
	public sealed class SourceFactory : ParameterizedSourceBase<Type, object>
	{
		public static IParameterizedSource<IActivator, SourceFactory> Defaults { get; } = new Cache<IActivator, SourceFactory>( func => new SourceFactory( func ) );
		public static SourceFactory Default { get; } = Defaults.Get( Activator.Default );

		readonly Func<Type, Delegate> factory;
		readonly IGenericMethodContext<Invoke> methods;

		SourceFactory( IActivator provider ) : this( new Factory( provider ).Get ) {}

		SourceFactory( Func<Type, Delegate> factory )
		{
			this.factory = factory;
			methods = GetType().Adapt().GenericFactoryMethods[nameof(ToResult)];
		}

		public override object Get( Type parameter )
		{
			var @delegate = factory( parameter );
			var result = @delegate != null ? methods.Make( ResultTypes.Default.Get( parameter ) ).Invoke<object>( @delegate ) : null;
			return result;
		}

		static object ToResult<T>( Func<T> source ) => source();

		sealed class Factory : CompositeFactory<Type, Delegate>
		{
			readonly static ImmutableArray<Func<IActivator, IParameterizedSource<Type, Delegate>>> Delegates = SourceDelegates.Sources.Append( ServiceProvidedParameterizedSourceDelegates.Sources ).Select( source => source.ToSourceDelegate() ).ToImmutableArray();
			public Factory( IActivator source ) : base( Delegates.Introduce( source ).ToArray() ) {}
		}
	}
}
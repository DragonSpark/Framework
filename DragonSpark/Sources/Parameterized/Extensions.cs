using DragonSpark.Coercion;
using DragonSpark.Expressions;
using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.Specifications;
using DragonSpark.TypeSystem.Generics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DragonSpark.Sources.Parameterized
{
	public static class Extensions
	{
		public static ImmutableArray<TResult> CreateMany<TParameter, TResult>( this IParameterizedSource<TParameter, TResult> @this, IEnumerable<TParameter> parameters ) =>
			parameters
				.SelectAssigned( @this.ToSourceDelegate() )
				.ToImmutableArray();

		public static IParameterizedSource<object, TResult> Apply<TParameter, TResult>( this IParameterizedSource<TParameter, TResult> @this, ICoercer<TParameter> coercer ) => Apply( @this.ToSourceDelegate(), coercer.ToDelegate() );
		public static IParameterizedSource<TFrom, TResult> Apply<TFrom, TParameter, TResult>( this IParameterizedSource<TParameter, TResult> @this, ICoercer<TFrom, TParameter> coercer ) => Apply( @this.ToSourceDelegate(), coercer.ToDelegate() );
		public static IParameterizedSource<TFrom, TResult> Apply<TFrom, TParameter, TResult>( this Func<TParameter, TResult> @this, Func<TFrom, TParameter> coerce ) =>
			new CoercedParameterizedSource<TFrom, TParameter, TResult>( coerce, @this );

		public static IParameterizedSource<TParameter, TResult> Apply<TParameter, TResult>( this IParameterizedSource<TParameter, TResult> @this, IAlteration<TParameter> alteration ) => Apply( @this, alteration.ToDelegate() );
		public static IParameterizedSource<TParameter, TResult> Apply<TParameter, TResult>( this IParameterizedSource<TParameter, TResult> @this, Alter<TParameter> alter ) => Apply( @this.ToSourceDelegate(), alter );
		public static IParameterizedSource<TParameter, TResult> Apply<TParameter, TResult>( this Func<TParameter, TResult> @this, Alter<TParameter> alter ) =>
			new AlteredParameterizedSource<TParameter, TResult>( alter, @this );

		public static IParameterizedSource<TParameter, TResult> Apply<TParameter, TResult>( this IParameterizedSource<TParameter, TResult> @this, IAlteration<TResult> selector ) => Apply( @this, selector.ToDelegate() );
		public static IParameterizedSource<TParameter, TResult> Apply<TParameter, TResult>( this IParameterizedSource<TParameter, TResult> @this, Alter<TResult> selector ) => Apply( @this.ToSourceDelegate(), selector );
		public static IParameterizedSource<TParameter, TResult> Apply<TParameter, TResult>( this Func<TParameter, TResult> @this, Alter<TResult> selector ) =>
			new AlteredResultParameterizedSource<TParameter, TResult>( selector, @this );

		public static ISpecificationParameterizedSource<TParameter, TResult> Apply<TParameter, TResult>( this IParameterizedSource<TParameter, TResult> @this, ISpecification<TParameter> specification ) =>
			new SpecificationParameterizedSource<TParameter, TResult>( specification, @this.ToSourceDelegate() );

		
		public static Func<object, T> Wrap<T>( this T @this ) => @this.Wrap<object, T>();

		public static Func<TParameter, TResult> Wrap<TParameter, TResult>( this TResult @this ) => Factory.For( @this ).Wrap<TParameter, TResult>();

		public static ISource<TResult> Fixed<TParameter, TResult>( this IParameterizedSource<TParameter, TResult> @this, TParameter parameter ) => @this.ToSourceDelegate().Fixed( parameter );
		public static ISource<TResult> Fixed<TParameter, TResult>( this IParameterizedSource<TParameter, TResult> @this, Func<TParameter> parameter ) => @this.ToSourceDelegate().Fixed( parameter );
		public static ISource<TResult> Fixed<TParameter, TResult>( this Func<TParameter, TResult> @this, TParameter parameter ) => @this.Fixed( Factory.For( parameter ) );

		public static ISource<TResult> Fixed<TParameter, TResult>( this Func<TParameter, TResult> @this, Func<TParameter> parameter ) => new SuppliedSource<TParameter, TResult>( @this, parameter );

		public static Func<object, T> Wrap<T>( this ISource<T> @this ) => @this.Wrap<object, T>();

		public static Func<TParameter, TResult> Wrap<TParameter, TResult>( this ISource<TResult> @this ) => new Func<TResult>( @this.Get ).Wrap<TParameter, TResult>();

		public static Func<object, T> Wrap<T>( this Func<T> @this ) => @this.Wrap<object, T>();

		public static Func<TParameter, TResult> Wrap<TParameter, TResult>( this Func<TResult> @this ) => new Wrapper<TParameter, TResult>( @this ).Get;

		public static Delegate Convert( this Func<object> @this, Type resultType ) => ConvertSupport.Methods.Make( resultType ).Invoke<Delegate>( @this );

		public static Delegate Convert( this Func<object, object> @this, Type parameterType, Type resultType ) => ConvertSupport.Methods.Make( parameterType, resultType ).Invoke<Delegate>( @this );

		static class ConvertSupport
		{
			public static IGenericMethodContext<Invoke> Methods { get; } = typeof(Extensions).Adapt().GenericFactoryMethods[nameof(Convert)];
		}

		public static Func<T> Convert<T>( this Func<object> @this ) => @this.Convert<object, T>();

		public static Func<object> Convert<T>( this Func<T> @this ) => Delegates<T>.Default.Get( @this );
		sealed class Delegates<T> : Cache<Func<T>, Func<object>>
		{
			public static Delegates<T> Default { get; } = new Delegates<T>();
			Delegates() : base( result => new Converter( result ).Get ) {}

			sealed class Converter : SourceBase<object>
			{
				readonly Func<T> @from;
				public Converter( Func<T> from )
				{
					this.@from = @from;
				}

				public override object Get() => from();
			}
		}

		public static Func<TTo> Convert<TFrom, TTo>( this Func<TFrom> @this ) where TTo : TFrom => Delegates<TFrom, TTo>.Default.Get( @this );
		sealed class Delegates<TFrom, TTo> : Cache<Func<TFrom>, Func<TTo>> where TTo : TFrom
		{
			public static Delegates<TFrom, TTo> Default { get; } = new Delegates<TFrom, TTo>();
			Delegates() : base( result => new Converter( result ).Get ) {}

			class Converter : SourceBase<TTo>
			{
				readonly Func<TFrom> @from;
				public Converter( Func<TFrom> from )
				{
					this.@from = @from;
				}

				public override TTo Get() => (TTo)from();
			}
		}

		public static Func<TParameter, TResult> Convert<TParameter, TResult>( this Func<object, object> @this ) => Convert<object, object, TParameter, TResult>( @this );
		public static Func<TToParameter, TToResult> Convert<TFromParameter, TFromResult, TToParameter, TToResult>( this Func<TFromParameter, TFromResult> @this ) => ParameterizedDelegates<TFromParameter, TFromResult, TToParameter, TToResult>.Default.Get( @this );
		sealed class ParameterizedDelegates<TFromParameter, TFromResult, TToParameter, TToResult> : Cache<Func<TFromParameter, TFromResult>, Func<TToParameter, TToResult>>
		{
			public static ParameterizedDelegates<TFromParameter, TFromResult, TToParameter, TToResult> Default { get; } = new ParameterizedDelegates<TFromParameter, TFromResult, TToParameter, TToResult>();
			ParameterizedDelegates() : base( result => new Converter( result ).To ) {}

			sealed class Converter 
			{
				readonly Func<TFromParameter, TFromResult> from;

				public Converter( Func<TFromParameter, TFromResult> from )
				{
					this.from = from;
				}

				public TToResult To( TToParameter parameter ) => (TToResult)(object)from( (TFromParameter)(object)parameter );
			}
		}

		public static Func<TParameter, TResult> ToSourceDelegate<TParameter, TResult>( this IParameterizedSource<TParameter, TResult> @this ) => ParameterizedSourceDelegates<TParameter, TResult>.Default.Get( @this );
		sealed class ParameterizedSourceDelegates<TParameter, TResult> : Cache<IParameterizedSource<TParameter, TResult>, Func<TParameter, TResult>>
		{
			public static ParameterizedSourceDelegates<TParameter, TResult> Default { get; } = new ParameterizedSourceDelegates<TParameter, TResult>();
			ParameterizedSourceDelegates() : base( factory => factory.Get ) {}
		}

		public static T Alter<T>( this IEnumerable<IAlteration<T>> @this, T seed ) => @this.Aggregate( seed, ( current, alteration ) => alteration.Get( current ) );

		public static Alter<T> ToDelegate<T>( this IAlteration<T> @this ) => Selectors<T>.Default.Get( @this );
		sealed class Selectors<T> : Cache<IAlteration<T>, Alter<T>>
		{
			public static Selectors<T> Default { get; } = new Selectors<T>();
			Selectors() : base( item => item.Get ) {}
		}

		public static ICache<T> ToCache<T>( this IParameterizedSource<object, T> @this ) => @this.ToSourceDelegate().ToCache();
		public static ICache<T> ToCache<T>( this Func<object, T> @this ) => ParameterizedSources<T>.Default.Get( @this );
		sealed class ParameterizedSources<T> : Cache<Func<object, T>, ICache<T>>
		{
			public static ParameterizedSources<T> Default { get; } = new ParameterizedSources<T>();
			ParameterizedSources() : base( CacheFactory.Create ) {}
		}

		public static ICache<TParameter, TResult> ToCache<TParameter, TResult>( this IParameterizedSource<TParameter, TResult> @this ) => @this.ToSourceDelegate().ToCache();
		public static ICache<TParameter, TResult> ToCache<TParameter, TResult>( this Func<TParameter, TResult> @this ) => ParameterizedSources<TParameter, TResult>.Default.Get( @this );
		sealed class ParameterizedSources<TParameter, TResult> : Cache<Func<TParameter, TResult>, ICache<TParameter, TResult>>
		{
			public static ParameterizedSources<TParameter, TResult> Default { get; } = new ParameterizedSources<TParameter, TResult>();
			ParameterizedSources() : base( CacheFactory.Create ) {}
		}

		public static ICache<TParameter, TResult> ToEqualityCache<TParameter, TResult>( this IParameterizedSource<TParameter, TResult> @this ) where TParameter : class => @this.ToSourceDelegate().ToEqualityCache();
		public static ICache<TParameter, TResult> ToEqualityCache<TParameter, TResult>( this Func<TParameter, TResult> @this ) where TParameter : class => new EqualityReferenceCache<TParameter, TResult>( @this );
	}
}
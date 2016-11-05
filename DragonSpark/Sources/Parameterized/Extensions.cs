using DragonSpark.Application;
using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.Specifications;
using DragonSpark.TypeSystem;
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
				.SelectAssigned( @this.ToDelegate() )
				.ToImmutableArray();

		public static IParameterizedSource<TParameter, TResult> Apply<TParameter, TResult>( this IParameterizedSource<TParameter, TResult> @this, IAlteration<TParameter> alteration ) => Apply( @this, alteration.ToDelegate() );
		public static IParameterizedSource<TParameter, TResult> Apply<TParameter, TResult>( this IParameterizedSource<TParameter, TResult> @this, Alter<TParameter> alter ) => Apply( @this.ToDelegate(), alter );
		public static IParameterizedSource<TParameter, TResult> Apply<TParameter, TResult>( this Func<TParameter, TResult> @this, Alter<TParameter> alter ) =>
			new AlteredParameterizedSource<TParameter, TResult>( alter, @this );

		public static IParameterizedSource<TParameter, TResult> Apply<TParameter, TResult>( this IParameterizedSource<TParameter, TResult> @this, Func<TResult, TResult> selector ) => Apply( @this, new Alter<TResult>( selector ) );
		public static IParameterizedSource<TParameter, TResult> Apply<TParameter, TResult>( this IParameterizedSource<TParameter, TResult> @this, IAlteration<TResult> selector ) => Apply( @this, selector.ToDelegate() );
		public static IParameterizedSource<TParameter, TResult> Apply<TParameter, TResult>( this IParameterizedSource<TParameter, TResult> @this, Alter<TResult> selector ) => Apply( @this.ToDelegate(), selector );
		public static IParameterizedSource<TParameter, TResult> Apply<TParameter, TResult>( this Func<TParameter, TResult> @this, Alter<TResult> selector ) =>
			new AlteredResultParameterizedSource<TParameter, TResult>( @this, selector );

		public static ISpecificationParameterizedSource<TParameter, TResult> Apply<TParameter, TResult>( this IParameterizedSource<TParameter, TResult> @this, ISpecification<TParameter> specification ) =>
			@this.Apply( specification.ToSpecificationDelegate() );
		public static ISpecificationParameterizedSource<TParameter, TResult> Apply<TParameter, TResult>( this IParameterizedSource<TParameter, TResult> @this, Func<TParameter, bool> specification ) =>
			new SpecificationParameterizedSource<TParameter, TResult>( specification, @this.ToDelegate() );
		
		public static ISource<TResult> WithParameter<TParameter, TResult>( this IParameterizedSource<TParameter, TResult> @this, TParameter parameter ) => @this.ToDelegate().WithParameter( parameter );
		public static ISource<TResult> WithParameter<TParameter, TResult>( this IParameterizedSource<TParameter, TResult> @this, Func<TParameter> parameter ) => @this.ToDelegate().WithParameter( parameter );
		public static ISource<TResult> WithParameter<TParameter, TResult>( this Func<TParameter, TResult> @this, TParameter parameter ) => @this.WithParameter( Factory.For( parameter ) );
		public static ISource<TResult> WithParameter<TParameter, TResult>( this Func<TParameter, TResult> @this, Func<TParameter> parameter ) => new SuppliedSource<TParameter, TResult>( @this, parameter );

		public static Func<object, T> Wrap<T>( this T @this ) => @this.Wrap<object, T>();
		public static Func<TParameter, TResult> Wrap<TParameter, TResult>( this TResult @this ) => Factory.For( @this ).Wrap<TParameter, TResult>();
		public static Func<TParameter, TResult> Wrap<TParameter, TResult>( this ISource<TResult> @this ) => new Func<TResult>( @this.Get ).Wrap<TParameter, TResult>();

		public static Func<object, T> Wrap<T>( this Func<T> @this ) => @this.Wrap<object, T>();

		public static Func<TParameter, TResult> Wrap<TParameter, TResult>( this Func<TResult> @this ) => new SourceAdapter<TParameter, TResult>( @this ).Get;

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

		public static Func<TParameter, TResult> ToDelegate<TParameter, TResult>( this IParameterizedSource<TParameter, TResult> @this ) => ParameterizedSourceDelegates<TParameter, TResult>.Default.Get( @this );
		sealed class ParameterizedSourceDelegates<TParameter, TResult> : Cache<IParameterizedSource<TParameter, TResult>, Func<TParameter, TResult>>
		{
			public static ParameterizedSourceDelegates<TParameter, TResult> Default { get; } = new ParameterizedSourceDelegates<TParameter, TResult>();
			ParameterizedSourceDelegates() : base( factory => factory.Get ) {}
		}

		public static Alter<T> ToDelegate<T>( this IAlteration<T> @this ) => Selectors<T>.Default.Get( @this );
		sealed class Selectors<T> : Cache<IAlteration<T>, Alter<T>>
		{
			public static Selectors<T> Default { get; } = new Selectors<T>();
			Selectors() : base( item => item.Get ) {}
		}

		public static T GetDefault<T>( this IParameterizedSource<object, T> @this ) => @this.ToDelegate().Invoke();
		public static T Invoke<T>( this Func<object, T> @this ) => @this.Invoke( Execution.Default.GetValue() );

		public static ICache<T> ToCache<T>( this IParameterizedSource<object, T> @this ) => @this.ToDelegate().ToCache();
		public static ICache<T> ToCache<T>( this Func<object, T> @this ) => ParameterizedSources<T>.Default.Get( @this );
		sealed class ParameterizedSources<T> : Cache<Func<object, T>, ICache<T>>
		{
			public static ParameterizedSources<T> Default { get; } = new ParameterizedSources<T>();
			ParameterizedSources() : base( Caches.Create ) {}
		}

		public static ICache<TParameter, TResult> ToCache<TParameter, TResult>( this IParameterizedSource<TParameter, TResult> @this ) => @this.ToDelegate().ToCache();
		public static ICache<TParameter, TResult> ToCache<TParameter, TResult>( this Func<TParameter, TResult> @this ) => ParameterizedSources<TParameter, TResult>.Default.Get( @this );
		sealed class ParameterizedSources<TParameter, TResult> : Cache<Func<TParameter, TResult>, ICache<TParameter, TResult>>
		{
			public static ParameterizedSources<TParameter, TResult> Default { get; } = new ParameterizedSources<TParameter, TResult>();
			ParameterizedSources() : base( Caches.Create ) {}
		}

		public static ICache<TParameter, TResult> ToEqualityCache<TParameter, TResult>( this IParameterizedSource<TParameter, TResult> @this ) where TParameter : class => @this.ToDelegate().ToEqualityCache();
		public static ICache<TParameter, TResult> ToEqualityCache<TParameter, TResult>( this Func<TParameter, TResult> @this ) where TParameter : class => new EqualityReferenceCache<TParameter, TResult>( @this );
		
		public static TResult Get<TItem, TResult>( this IParameterizedSource<ImmutableArray<TItem>, TResult> @this ) => @this.ToDelegate().Get();
		public static TResult Get<TItem, TResult>( this IParameterizedSource<ImmutableArray<TItem>, TResult> @this, params TItem[] parameters ) => @this.ToDelegate().Get( parameters );
		public static TResult Get<TItem, TResult>( this Func<ImmutableArray<TItem>, TResult> @this ) => @this.Get( Items<TItem>.Default );
		public static TResult Get<TItem, TResult>( this Func<ImmutableArray<TItem>, TResult> @this, params TItem[] parameters ) => @this( parameters.ToImmutableArray() );
		public static TResult Get<TItem, TResult>( this IParameterizedSource<IEnumerable<TItem>, TResult> @this ) => @this.ToDelegate().Get();
		public static TResult Get<TItem, TResult>( this IParameterizedSource<IEnumerable<TItem>, TResult> @this, params TItem[] parameters ) => @this.ToDelegate().Get( parameters );
		public static TResult Get<TItem, TResult>( this Func<IEnumerable<TItem>, TResult> @this ) => Get( @this, Items<TItem>.Default );
		public static TResult Get<TItem, TResult>( this Func<IEnumerable<TItem>, TResult> @this, params TItem[] parameters ) => @this( parameters );

		public static TResult GetImmutable<TItem, TResult>( this IParameterizedSource<IEnumerable<TItem>, TResult> @this, ImmutableArray<TItem> parameter ) => @this.ToDelegate().GetImmutable( parameter );
		public static TResult GetImmutable<TItem, TResult>( this Func<IEnumerable<TItem>, TResult> @this, ImmutableArray<TItem> parameter ) => @this( parameter.ToArray() );
		public static ImmutableArray<TItem> GetImmutable<TParameter, TItem>( this IParameterizedSource<TParameter, IEnumerable<TItem>> @this, TParameter parameter ) => GetImmutable( @this.ToDelegate(), parameter );
		public static ImmutableArray<TItem> GetImmutable<TParameter, TItem>( this Func<TParameter, IEnumerable<TItem>> @this, TParameter parameter ) => @this( parameter ).ToImmutableArray();

		public static TResult GetEnumerable<TItem, TResult>( this IParameterizedSource<ImmutableArray<TItem>, TResult> @this, IEnumerable<TItem> parameter ) => @this.ToDelegate().GetEnumerable( parameter );
		public static TResult GetEnumerable<TItem, TResult>( this Func<ImmutableArray<TItem>, TResult> @this, IEnumerable<TItem> parameter ) => @this( parameter.ToImmutableArray() );
		public static IEnumerable<TItem> GetEnumerable<TParameter, TItem>( this IParameterizedSource<TParameter, ImmutableArray<TItem>> @this, TParameter parameter ) => @this.ToDelegate().GetEnumerable( parameter );
		public static IEnumerable<TItem> GetEnumerable<TParameter, TItem>( this Func<TParameter, ImmutableArray<TItem>> @this, TParameter parameter ) => @this( parameter ).ToArray();
	}
}
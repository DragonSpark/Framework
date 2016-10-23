using DragonSpark.Commands;
using DragonSpark.Extensions;
using DragonSpark.Runtime.Assignments;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DragonSpark.Sources
{
	public static class Extensions
	{
		// public static IDisposable Assignment<T>( this IScope<T> @this, T start ) => new ValueAssignment<Func<T>>( new Assign<Func<T>>( @this ), new Value<Func<T>>( Factory.For( start ), Factory.For( @this.Get() ) ) );

		public static T[] Unwrap<T>( this ISource<ImmutableArray<T>> @this ) => @this.Get().ToArray();

		public static void Assign<T>( this IAssignable<ImmutableArray<T>> @this, params T[] parameter ) => @this.Assign( (IEnumerable<T>)parameter );
		public static void Assign<T>( this IAssignable<ImmutableArray<T>> @this, IEnumerable<T> parameter ) => @this.Assign( parameter.ToImmutableArray() );

		public static object Value( this Func<object> @this ) => @this().Value<object>();
		public static object Value( this object @this ) => @this.Value<object>();
		public static T Value<T>( this object @this ) => SourceCoercer<T>.Default.Coerce( @this );

		// public static ICommand Configured<T>( this IAssignable<T> @this, T value ) => new AssignCommand<T>( @this ).Fixed( value );

		public static void Assign<TParameter, TResult>( this IParameterizedScope<TParameter, TResult> @this, Func<TParameter, TResult> instance ) => @this.Assign( instance.Self );

		public static void Assign<T>( this IScopeAware<T> @this, T instance ) => @this.Assign( Factory.For( instance ) );

		public static T WithInstance<T>( this IScope<T> @this, T instance )
		{
			@this.Assign( instance );
			return @this.Get();
		}

		public static T WithFactory<T>( this IScope<T> @this, Func<T> factory )
		{
			@this.Assign( factory );
			return @this.Get();
		}

		/*public static T ScopedWithDefault<T>( this T @this ) where T : IScopeAware => @this.ScopedWith( ExecutionContext.Default );*/

		public static T ScopedWith<T>( this T @this, ISource scope ) where T : IScopeAware
		{
			@this.Assign( scope );
			return @this;
		}

		// public static IRunCommand Configured<T>( this IAssignable<Func<T>> @this, ISource<T> source ) => @this.Configured( source.ToDelegate() );
		public static IRunCommand Configured<T>( this IAssignable<Func<T>> @this, T instance ) => @this.Configured( Factory.For( instance ) );
		public static IRunCommand Configured<T>( this IAssignable<Func<T>> @this, Func<T> factory ) => new AssignCommand<Func<T>>( @this ).Fixed( factory );
		// public static IRunCommand Configured<T>( this IAssignable<Func<object, T>> @this, Func<object, T> factory ) => new AssignCommand<Func<object, T>>( @this ).Fixed( factory );

		public static void Assign<T>( this IAssignable<Func<object, T>> @this ) where T : class, new() => @this.Assign( o => new T() );

		/*public static Func<TParameter, TResult> Local<TParameter, TResult>( this ISource<IParameterizedSource<TParameter, TResult>> @this ) => @this.GetCurrentDelegate();
		public static Func<TParameter, TResult> Global<TParameter, TResult>( this ISource<IParameterizedSource<TParameter, TResult>> @this, object _ ) => @this.GetCurrentDelegate();*/

		public static TResult GetCurrent<TParameter, TResult>( this ISource<IParameterizedSource<TParameter, TResult>> @this, TParameter parameter ) => @this.Get().Get( parameter );
		public static Func<TParameter, TResult> GetCurrentDelegate<TParameter, TResult>( this ISource<IParameterizedSource<TParameter, TResult>> @this ) => LocalDelegates<TParameter, TResult>.Default.Get( @this );
		sealed class LocalDelegates<TParameter, TResult> : Cache<ISource<IParameterizedSource<TParameter, TResult>>, Func<TParameter, TResult>>
		{
			public static LocalDelegates<TParameter, TResult> Default { get; } = new LocalDelegates<TParameter, TResult>();
			LocalDelegates() : base( source => source.GetCurrent ) {}
		}

		public static object GetCurrent( this ISource<ISource> @this ) => @this.Get().Get();
		public static T GetCurrent<T>( this ISource<ISource<T>> @this ) => @this.Get().Get();
		/*public static Func<Func<object>> LocalDelegate( this ISource<ISource> @this ) => @this.ToDelegate().LocalDelegate();
		public static Func<Func<object>> LocalDelegate( this Func<ISource> @this ) => Delegates.Default.Get( @this );
		sealed class Delegates : Cache<Func<ISource>, Func<Func<object>>>
		{
			public static Delegates Default { get; } = new Delegates();
			Delegates() : base( source => source.Self ) {}
		}*/
		public static Func<Func<TParameter, TResult>> Local<TParameter, TResult>( this ISource<ISource<TResult>> @this ) 
			=> @this.GetCurrentDelegate().Wrap<TParameter, TResult>().Self;
		public static Func<object, Func<TParameter,TResult>> Global<TParameter, TResult>( this ISource<ISource<TResult>> @this ) 
			=> @this.GetCurrentDelegate().Wrap<TParameter, TResult>().Wrap();

		public static Func<T> GetCurrentDelegate<T>( this ISource<ISource<T>> @this ) => Delegates<T>.Default.Get( @this );
		sealed class Delegates<T> : Cache<ISource<ISource<T>>, Func<T>>
		{
			public static Delegates<T> Default { get; } = new Delegates<T>();
			Delegates() : base( source => source.GetCurrent ) {}
		}

		public static Func<T> ToDelegate<T>( this ISource<T> @this ) => ParameterizedSourceDelegates<T>.Default.Get( @this );
		sealed class ParameterizedSourceDelegates<T> : Cache<ISource<T>, Func<T>>
		{
			public static ParameterizedSourceDelegates<T> Default { get; } = new ParameterizedSourceDelegates<T>();
			ParameterizedSourceDelegates() : base( factory => factory.Get ) {}
		}

		public static Func<TParameter, TResult> Timed<TParameter, TResult>( this IParameterizedSource<TParameter, TResult> @this ) => @this.ToSourceDelegate().Timed();
		public static Func<TParameter, TResult> Timed<TParameter, TResult>( this Func<TParameter, TResult> @this ) => Timed( @this, Defaults.ParameterizedTimerTemplate );
		public static Func<TParameter, TResult> Timed<TParameter, TResult>( this Func<TParameter, TResult> @this, string template ) => new TimedDelegatedSource<TParameter, TResult>( @this, template ).Get;

		public static Func<T> Timed<T>( this ISource<T> @this ) => @this.ToDelegate().Timed();
		public static Func<T> Timed<T>( this Func<T> @this ) => Timed( @this, Defaults.TimerTemplate );
		public static Func<T> Timed<T>( this Func<T> @this, string template ) => new TimedDelegatedSource<T>( @this, template ).Get;
		

		public static IScope<T> ToScope<T>( this IParameterizedSource<object, T> @this ) => @this.ToSourceDelegate().ToScope();
		public static IScope<T> ToScope<T>( this Func<object, T> @this ) => Scopes<T>.Default.Get( @this );
		sealed class Scopes<T> : Cache<Func<object, T>, IScope<T>>
		{
			public static Scopes<T> Default { get; } = new Scopes<T>();
			Scopes() : base( cache => new Scope<T>( cache.Cache() ) ) {}
		}

		/*public static ISource<T> Fixed<T>( this ISource<T> @this ) => SuppliedSources<T>.Default.Get( @this );
		sealed class SuppliedSources<T> : Cache<ISource<T>, ISource<T>>
		{
			public static SuppliedSources<T> Default { get; } = new SuppliedSources<T>();
			SuppliedSources() : base( source => new SuppliedDeferredSource<T>( source.Get ) ) {}
		}*/

		public static IEnumerable<T> AsEnumerable<T>( this ISource<ImmutableArray<T>> @this ) => EnumerableSource<T>.Sources.Get( @this )();
		sealed class EnumerableSource<T> : SourceBase<IEnumerable<T>>
		{
			public static IParameterizedSource<ISource<ImmutableArray<T>>, Func<IEnumerable<T>>> Sources { get; } = new Cache<ISource<ImmutableArray<T>>, Func<IEnumerable<T>>>( s => new EnumerableSource<T>( s ).Get );

			readonly ISource<ImmutableArray<T>> source;

			EnumerableSource( ISource<ImmutableArray<T>> source )
			{
				this.source = source;
			}

			public override IEnumerable<T> Get() => source.Get().AsEnumerable();
		}
	}
}
using DragonSpark.Commands;
using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices;

namespace DragonSpark.Sources
{
	public static class Extensions
	{
		public static T Account<T>( [Optional]this object @this ) => Account( @this, typeof(T) ).As<T>();
		public static object Account( [Optional]this object @this, Type type ) => @this.IsAssigned() ? SourceAccountedAlteration.Defaults.Get( type ).Invoke( @this ) : @this;

		public static T[] Unwrap<T>( this ISource<ImmutableArray<T>> @this ) => @this.Get().ToArray();

		public static TResult GetValue<TParameter, TResult>( this ISource<IParameterizedSource<TParameter, TResult>> @this, TParameter parameter ) => @this.Get().Get( parameter );
		public static Func<TParameter, TResult> GetValueDelegate<TParameter, TResult>( this ISource<IParameterizedSource<TParameter, TResult>> @this ) => ValueDelegates<TParameter, TResult>.Default.Get( @this );
		sealed class ValueDelegates<TParameter, TResult> : Cache<ISource<IParameterizedSource<TParameter, TResult>>, Func<TParameter, TResult>>
		{
			public static ValueDelegates<TParameter, TResult> Default { get; } = new ValueDelegates<TParameter, TResult>();
			ValueDelegates() : base( source => source.GetValue ) {}
		}

		public static object GetValue( this ISource<ISource> @this ) => @this.Get().Get();
		public static T GetValue<T>( this ISource<ISource<T>> @this ) => @this.Get().Get();
		public static Func<T> GetValueDelegate<T>( this ISource<ISource<T>> @this ) => ValueDelegates<T>.Default.Get( @this );
		sealed class ValueDelegates<T> : Cache<ISource<ISource<T>>, Func<T>>
		{
			public static ValueDelegates<T> Default { get; } = new ValueDelegates<T>();
			ValueDelegates() : base( source => source.GetValue ) {}
		}

		public static Func<T> ToDelegate<T>( this ISource<T> @this ) => ParameterizedSourceDelegates<T>.Default.Get( @this );
		sealed class ParameterizedSourceDelegates<T> : Cache<ISource<T>, Func<T>>
		{
			public static ParameterizedSourceDelegates<T> Default { get; } = new ParameterizedSourceDelegates<T>();
			ParameterizedSourceDelegates() : base( factory => factory.Get ) {}
		}

		public static Func<TParameter, TResult> Timed<TParameter, TResult>( this IParameterizedSource<TParameter, TResult> @this ) => @this.ToDelegate().Timed();
		public static Func<TParameter, TResult> Timed<TParameter, TResult>( this Func<TParameter, TResult> @this ) => Timed( @this, Defaults.ParameterizedTimerTemplate );
		public static Func<TParameter, TResult> Timed<TParameter, TResult>( this Func<TParameter, TResult> @this, string template ) => new TimedDelegatedSource<TParameter, TResult>( @this, template ).Get;

		public static Func<T> Timed<T>( this ISource<T> @this ) => @this.ToDelegate().Timed();
		public static Func<T> Timed<T>( this Func<T> @this ) => Timed( @this, Defaults.TimerTemplate );
		public static Func<T> Timed<T>( this Func<T> @this, string template ) => new TimedDelegatedSource<T>( @this, template ).Get;
		

		public static IRunCommand ToCommand<T>( this ISource<T> @this ) => @this.ToDelegate().ToCommand();
		public static IRunCommand ToCommand<T>( this Func<T> @this ) => Commands<T>.Default.Get( @this );
		sealed class Commands<T> : Cache<Func<T>, IRunCommand>
		{
			public static Commands<T> Default { get; } = new Commands<T>();
			Commands() : base( x => new FactoryCommand<T>( x ) ) {}
		}

		public static ImmutableArray<T> GetImmutable<T>( this ISource<IEnumerable<T>> @this ) => @this.Get().ToImmutableArray();
		public static IEnumerable<T> GetEnumerable<T>( this ISource<ImmutableArray<T>> @this ) => @this.Get().ToArray();

		public static IEnumerable<T> IncludeExports<T>( this IEnumerable<T> @this ) => IncludeExportsAlteration<T>.Default.Get( @this );

		public static IItemSource<T> ToSource<T>( this IEnumerable<T> @this ) => new ItemSource<T>( @this );
	}
}
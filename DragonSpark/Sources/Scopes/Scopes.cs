using DragonSpark.Commands;
using DragonSpark.Runtime.Assignments;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace DragonSpark.Sources.Scopes
{
	public static class Scopes
	{
		public static IScope<T> ToScope<T>( this ISource<T> @this ) => @this.ToDelegate().ToScope();
		public static IScope<T> ToScope<T>( this Func<T> @this ) => new Scope<T>( @this );
		public static IParameterizedScope<TParameter, TResult> ToScope<TParameter, TResult>( this IParameterizedSource<TParameter, TResult> @this ) => @this.ToDelegate().ToScope();
		public static IParameterizedScope<TParameter, TResult> ToScope<TParameter, TResult>( this Func<TParameter, TResult> @this ) => new ParameterizedScope<TParameter, TResult>( @this );
		public static IScope<T> ToSingletonScope<T>( this ISource<T> @this ) => @this.ToDelegate().ToSingletonScope();
		public static IScope<T> ToSingletonScope<T>( this Func<T> @this ) => new SingletonScope<T>( @this );
		public static IParameterizedScope<TParameter, TResult> ToSingletonScope<TParameter, TResult>( this IParameterizedSource<TParameter, TResult> @this ) => @this.ToDelegate().ToSingletonScope();
		public static IParameterizedScope<TParameter, TResult> ToSingletonScope<TParameter, TResult>( this TResult @this ) => @this.Wrap<TParameter, TResult>().ToSingletonScope();
		public static IParameterizedScope<TParameter, TResult> ToSingletonScope<TParameter, TResult>( this Func<TParameter, TResult> @this ) => new ParameterizedSingletonScope<TParameter, TResult>( @this );
		public static IScope<T> ToExecutionScope<T>( this IParameterizedSource<object, T> @this ) => @this.ToDelegate().ToExecutionScope();
		public static IScope<T> ToExecutionScope<T>( this Func<object, T> @this ) => ExecutionScopes<T>.Default.Get( @this );
		sealed class ExecutionScopes<T> : Cache<Func<object, T>, IScope<T>>
		{
			public static ExecutionScopes<T> Default { get; } = new ExecutionScopes<T>();
			ExecutionScopes() : base( factory => new Scope<T>( factory ) ) {}
		}
		public static IScope<T> ToExecutionSingletonScope<T>( this IParameterizedSource<object, T> @this ) => @this.ToDelegate().ToExecutionSingletonScope();
		public static IScope<T> ToExecutionSingletonScope<T>( this Func<object, T> @this ) => ExecutionSingletonScopes<T>.Default.Get( @this );
		sealed class ExecutionSingletonScopes<T> : Cache<Func<object, T>, IScope<T>>
		{
			public static ExecutionSingletonScopes<T> Default { get; } = new ExecutionSingletonScopes<T>();
			ExecutionSingletonScopes() : base( factory => new SingletonScope<T>( factory ) ) {}
		}

		public static Func<T> ToSingleton<T>( this ISource<T> @this ) => @this.ToDelegate().ToSingleton();
		public static Func<T> ToSingleton<T>( this Func<T> @this ) => SingletonDelegateBuilder<T>.Default.Get( @this );
		public static Func<TParameter, TResult> ToSingleton<TParameter, TResult>( this IParameterizedSource<TParameter, TResult> @this ) => @this.ToDelegate().ToSingleton();
		public static Func<TParameter, TResult> ToSingleton<TParameter, TResult>( this Func<TParameter, TResult> @this ) => Caches.Create( @this ).Get;

		public static Func<TParameter, TResult> Invoke<TParameter, TResult>( this Func<TParameter, TResult> @this, object _ ) => @this;
		public static T Scoped<T>( this ISource<T> @this, object _ ) => @this.ToDelegate().Scoped( _ );
		public static T Scoped<T>( this Func<T> @this, object _ ) => @this();
		public static Func<TParameter, TResult> Scoped<TParameter, TResult>( this Func<TParameter, TResult> @this, object _ ) => @this.ToSingleton();
		public static ImmutableArray<IAlteration<T>> Scoped<T>( this IItemSource<IAlteration<T>> @this, object _ ) => @this.Get();
		
		// public static ImmutableArray<IAlteration<TResult>> Get<TParameter, TResult>( this IEnumerable<IAlteration<TResult>> @this, TParameter _ ) => @this.ToImmutableArray();
		// public static ImmutableArray<IAlteration<TResult>> GetValue<TParameter, TResult>( this IParameterizedSource<TParameter, IAlterations<TResult>> @this, TParameter parameter ) => @this.Get( parameter ).Get();
		/*public static ImmutableArray<IAlteration<TResult>> Invoke<TParameter, TResult>( this IParameterizedSource<TParameter, IAlterations<TResult>> @this, TParameter parameter ) => @this.Get( parameter ).Get();*/
		// public static IAlteration<T>[] Unwrap<T>( this ISource<IAlterations<T>> @this ) => @this.GetValue().ToArray();
		
		public static void Assign<T>( this IAssignable<Func<object, T>> @this ) where T : class, new() => @this.Assign( o => new T() );
		public static void Assign<T>( this IAssignable<ImmutableArray<T>> @this, params T[] parameter ) => @this.Assign( (IEnumerable<T>)parameter );
		public static void Assign<T>( this IAssignable<ImmutableArray<T>> @this, IEnumerable<T> parameter ) => @this.Assign( parameter.ToImmutableArray() );
		public static void Assign<TParameter, TResult>( this IParameterizedScope<TParameter, TResult> @this, Func<TParameter, TResult> instance ) => @this.Assign( instance.Self );
		public static void Assign<T>( this IScopeAware<T> @this, T instance ) => @this.Assign( Factory.For( instance ) );
		public static void Assign<T>( this IScopeAware<T?> @this, T instance ) where T : struct => @this.Assign( Factory.For( new T?( instance ) ) );

		public static IRunCommand ToCommand<T>( this IAssignable<Func<T>> @this, T instance ) => @this.ToCommand( Factory.For( instance ) );
		public static IRunCommand ToCommand<T>( this IAssignable<Func<T>> @this, Func<T> factory ) => new AssignCommand<Func<T>>( @this ).WithParameter( factory );
		
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
	}
}
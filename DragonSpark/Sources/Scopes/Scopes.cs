using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using DragonSpark.Commands;
using DragonSpark.Runtime.Assignments;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;

namespace DragonSpark.Sources.Scopes
{
	public static class Scopes
	{
		public static IScope<T> Create<T>( this ISource<T> @this ) => @this.ToDelegate().Create();
		public static IScope<T> Create<T>( this Func<T> @this ) => new Scope<T>( @this.Singleton() );
		public static Func<T> ToScopeDelegate<T>( this ISource<T> @this ) => @this.ToDelegate().ToScopeDelegate();
		public static Func<T> ToScopeDelegate<T>( this Func<T> @this ) => @this.Create().ToDelegate();

		public static T AssignLocal<T>( this ISource<T> @this ) => @this.Get();
		public static T AssignGlobal<T>( this ISource<T> @this, object _ ) => @this.Get();
		public static T AssignGlobal<T>( this ISource<ISource<T>> @this, object _ ) => @this.GetValue();

		public static Func<TParameter,TResult> AssignGlobal<TParameter, TResult>( this IParameterizedSource<TParameter, TResult> @this, object _ ) => @this.ToDelegate();
		public static Func<TParameter,TResult> AssignGlobal<TParameter, TResult>( this Func<TParameter, TResult> @this, object _ ) => CacheFactory.Create( @this ).Get;

		public static IParameterizedScope<TParameter, TResult> Create<TParameter, TResult>( this IParameterizedSource<TParameter, TResult> @this ) => @this.ToDelegate().Create();
		public static IParameterizedScope<TParameter, TResult> Create<TParameter, TResult>( this Func<TParameter, TResult> @this ) => new ParameterizedScope<TParameter, TResult>( @this.Singleton() );

		public static IScope<T> ToExecutionScope<T>( this IParameterizedSource<object, T> @this ) => @this.ToDelegate().ToExecutionScope();
		public static IScope<T> ToExecutionScope<T>( this Func<object, T> @this ) => ScopeCaches<T>.Default.Get( @this );
		sealed class ScopeCaches<T> : Cache<Func<object, T>, IScope<T>>
		{
			public static ScopeCaches<T> Default { get; } = new ScopeCaches<T>();
			ScopeCaches() : base( cache => new Scope<T>( cache.Cache() ) ) {}
		}

		public static void Assign<T>( this IAssignable<Func<object, T>> @this ) where T : class, new() => @this.Assign( new Func<object, T>( o => new T() ).Cache() );
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
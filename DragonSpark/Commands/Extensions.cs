using DragonSpark.Extensions;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.Specifications;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using ICommand = System.Windows.Input.ICommand;

namespace DragonSpark.Commands
{
	public static class Extensions
	{
		public static ICommand<T> Apply<T>( this ICommand<T> @this, ISpecification<T> specification ) => new SpecificationCommand<T>( specification, @this.ToDelegate() );

		public static CoercedCommand<TFrom, TParameter> Accept<TFrom, TParameter>( this ICommand<TParameter> @this, IParameterizedSource<TFrom, TParameter> coercer ) => @this.ToDelegate().Accept( coercer );
		public static CoercedCommand<TFrom, TParameter> Accept<TFrom, TParameter>( this Action<TParameter> @this, IParameterizedSource<TFrom, TParameter> coercer ) => new CoercedCommand<TFrom,TParameter>( coercer, @this );

		public static void Execute<T>( this ISource<ICommand<T>> @this, T parameter ) => @this.ToDelegate().Execute( parameter );
		public static void Execute<T>( this Func<ICommand<T>> @this, T parameter ) => @this().Execute( parameter );

		public static void Execute<T>( this ISource<Action<T>> @this, T parameter ) => @this.ToDelegate().Execute( parameter );
		public static void Execute<T>( this Func<Action<T>> @this, T parameter ) => @this().Invoke( parameter );

		public static void ExecuteItem<T>( this ICommand<ImmutableArray<T>> @this, T parameter ) => Execute( @this, parameter.Yield() );
		public static void Execute<T>( this ICommand<ImmutableArray<T>> @this, IEnumerable<T> parameter ) => Execute( @this, parameter.Fixed() );
		public static void Execute<T>( this ICommand<ImmutableArray<T>> @this, params T[] parameter ) => @this.Execute( parameter.ToImmutableArray() );

		public static IDisposable AsExecuted( this IExecution @this )
		{
			@this.Execute();
			return @this;
		}

		public static Action<T> ToExecuteDelegate<T>( this ISource<Action<T>> @this ) => ActionDelegates<T>.Default.Get( @this );
		sealed class ActionDelegates<T> : Cache<ISource<Action<T>>, Action<T>>
		{
			public static ActionDelegates<T> Default { get; } = new ActionDelegates<T>();
			ActionDelegates() : base( command => command.Execute ) {}
		}

		public static Action<T> ToExecuteDelegate<T>( this ISource<ICommand<T>> @this ) => ExecuteDelegates<T>.Default.Get( @this );
		sealed class ExecuteDelegates<T> : Cache<ISource<ICommand<T>>, Action<T>>
		{
			public static ExecuteDelegates<T> Default { get; } = new ExecuteDelegates<T>();
			ExecuteDelegates() : base( command => command.Execute ) {}
		}

		public static ICommand<T> Adapt<T>( this ICommand @this ) => new AdapterCommand<T>( @this );

		public static ICommand<ImmutableArray<T>> AsCompiled<T>( this ICommand<T> @this ) => @this.ToDelegate().AsCompiled();
		public static ICommand<ImmutableArray<T>> AsCompiled<T>( this Action<T> @this ) => Compiled<T>.Default.Get( @this );
		sealed class Compiled<T> : Cache<Action<T>, ICommand<ImmutableArray<T>>>
		{
			public static Compiled<T> Default { get; } = new Compiled<T>();
			Compiled() : base( source => new CompiledCommand<T>( source ) ) {}
		}

		public static IConfigurableCommand<T> AsConfigurable<T>( this ICommand<T> @this ) => Configurable<T>.Default.Get( @this );
		sealed class Configurable<T> : Cache<ICommand<T>, IConfigurableCommand<T>>
		{
			public static Configurable<T> Default { get; } = new Configurable<T>();
			Configurable() : base( source => new ScopedCommand<T>( source.Execute ) ) {}
		}

		public static Action ToRunDelegate( this IRunCommand @this ) => RunDelegates.Default.Get( @this );
		sealed class RunDelegates : Cache<IRunCommand, Action>
		{
			public static RunDelegates Default { get; } = new RunDelegates();
			RunDelegates() : base( command => command.Execute ) {}
		}

		public static TCommand Run<TCommand, TParameter>( this TCommand @this, TParameter parameter ) where TCommand : ICommand<TParameter>
		{
			var result = @this.IsSatisfiedBy( parameter ) ? @this : default(TCommand);
			result?.Execute( parameter );
			return result;
		}

		public static SuppliedCommand<T> WithParameter<T>( this ICommand<T> @this, T parameter ) => new SuppliedCommand<T>( @this, parameter );
		public static SuppliedCommand<T> WithParameter<T>( this ICommand<T> @this, Func<T> parameter ) => new SuppliedCommand<T>( @this, parameter );
		public static SuppliedCommand<T> WithParameter<T>( this Action<T> @this, T parameter ) => new SuppliedCommand<T>( @this, parameter );
		public static SuppliedCommand<T> WithParameter<T>( this Action<T> @this, Func<T> parameter ) => new SuppliedCommand<T>( @this, parameter );

		public static Action<T> ToDelegate<T>( this ICommand<T> @this ) => DelegateCache<T>.Default.Get( @this );
		sealed class DelegateCache<T> : Cache<ICommand<T>, Action<T>>
		{
			public static DelegateCache<T> Default { get; } = new DelegateCache<T>();
			DelegateCache() : base( command => command.Execute ) {}
		}
		
		public static IAlteration<T> ToAlteration<T>( this ICommand<T> @this ) => Alterations<T>.Default.Get( @this );
		sealed class Alterations<T> : Cache<ICommand<T>, IAlteration<T>>
		{
			public static Alterations<T> Default { get; } = new Alterations<T>();
			Alterations() : base( command => new ConfiguringAlteration<T>( command.ToDelegate() ) ) {}
		}

		public static ISpecification<object> ToSpecification( this ICommand @this ) => SpecificationCache.Default.Get( @this );
		class SpecificationCache : Cache<ICommand, ISpecification<object>>
		{
			public static SpecificationCache Default { get; } = new SpecificationCache();
			SpecificationCache() : base( command => new DelegatedSpecification<object>( command.CanExecute ) ) {}
		}

		public static ISpecification<T> ToSpecification<T>( this ICommand<T> @this ) => SpecificationCache<T>.Default.Get( @this );
		class SpecificationCache<T> : Cache<ICommand<T>, ISpecification<T>>
		{
			public static SpecificationCache<T> Default { get; } = new SpecificationCache<T>();
			SpecificationCache() : base( command => new DelegatedSpecification<T>( command.IsSatisfiedBy ) ) {}
		}

		public static Action<T> Wrap<T>( this Action @this ) => Wrappers<T>.Default.Get( @this );
		sealed class Wrappers<T> : Cache<Action, Action<T>>
		{
			public static Wrappers<T> Default { get; } = new Wrappers<T>();
			Wrappers() : base( result => new Wrapper<T>( result ).Execute ) {}
		}

		sealed class Wrapper<T> : CommandBase<T>
		{
			readonly Action action;

			public Wrapper( Action action )
			{
				this.action = action;
			}

			public override void Execute( T parameter ) => action();
		}

		public static Action<T> Timed<T>( this ICommand<T> @this ) => @this.ToDelegate().Timed();
		public static Action<T> Timed<T>( this Action<T> @this ) => Timed( @this, Sources.Defaults.ParameterizedTimerTemplate );
		public static Action<T> Timed<T>( this Action<T> @this, string template ) => new TimedDelegatedCommand<T>( @this, template ).Execute;
	}
}

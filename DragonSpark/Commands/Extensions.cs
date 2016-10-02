using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.Specifications;
using System;
using ICommand = System.Windows.Input.ICommand;

namespace DragonSpark.Commands
{
	public static class Extensions
	{
		public static ICommand<T> Adapt<T>( this ICommand @this ) => new AdapterCommand<T>( @this );

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

		public static SuppliedCommand<T> Fixed<T>( this ICommand<T> @this, T parameter ) => new SuppliedCommand<T>( @this, parameter );
		public static SuppliedCommand<T> Fixed<T>( this ICommand<T> @this, Func<T> parameter ) => new SuppliedCommand<T>( @this, parameter );

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

using DragonSpark.Activation;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Aspects;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using PostSharp.Patterns.Contracts;
using System;
using System.Linq;
using DragonSpark.Activation.IoC;

namespace DragonSpark.Setup.Registration
{
	public class FactoryRegistration : IRegistration
	{
		readonly Type factoryType;

		public FactoryRegistration( [Required, OfFactoryType]Type factoryType )
		{
			this.factoryType = factoryType;
		}

		public void Register( IServiceRegistry registry )
		{
			new ICommand<Type>[] { new RegisterFactoryWithParameterCommand( registry ), new RegisterFactoryCommand( registry ) }
				.FirstOrDefault( command => command.CanExecute( factoryType ) )
				.With( command => command.Run( factoryType ) );
		}
	}

	public static class ServiceRegistryExtensions
	{
		public static void Register<TFrom, TTo>( this IServiceRegistry @this, string name = null ) => @this.Register( typeof(TFrom), typeof(TTo), name );

		public static void Register<TService>( this IServiceRegistry @this, TService instance ) => @this.Register( typeof(TService), instance );

		public static void Register<TService>( this IServiceRegistry @this, Func<TService> factory ) => @this.RegisterFactory( typeof(TService), () => factory() );

		public static IServiceRegistry RegisterFactory( [Required] this IServiceRegistry @this, [Required]IFactory factory )
		{
			new RegisterFactoryCommand( @this, new FactoryDelegateFactory( factory ).Create )
				.ExecuteWith( factory.GetType() );
			return @this;
		}

		public static IServiceRegistry RegisterFactory( [Required] this IServiceRegistry @this, [Required]IFactoryWithParameter factory )
		{
			new RegisterFactoryWithParameterCommand( @this, new FactoryWithParameterContainedDelegateFactory( type => factory.Create ).Create )
				.ExecuteWith( factory.GetType() );
			return @this;
		}
	}

	public class FactoryDelegateFactory : FactoryBase<Type, Func<object>>
	{
		readonly Func<Type, IFactory> createFactory;

		public static FactoryDelegateFactory Instance { get; } = new FactoryDelegateFactory();

		public FactoryDelegateFactory() : this( ActivateFactory<IFactory>.Instance.CreateUsing ) {}

		public FactoryDelegateFactory( IFactory factory ) : this( t => factory ) {}

		public FactoryDelegateFactory( [Required]Func<Type, IFactory> createFactory )
		{
			this.createFactory = createFactory;
		}

		protected override Func<object> CreateItem( Type parameter ) => () =>
		{
			var result = createFactory( parameter )?.Create();
			return result;
		};
	}

	public class FactoryWithParameterDelegateFactory : FactoryBase<Type, Func<object, object>>
	{
		public static FactoryWithParameterDelegateFactory Instance { get; } = new FactoryWithParameterDelegateFactory();

		readonly Func<Type, IFactoryWithParameter> createFactory;

		public FactoryWithParameterDelegateFactory() : this( ActivateFactory<IFactoryWithParameter>.Instance.CreateUsing ) {}

		public FactoryWithParameterDelegateFactory( [Required]Func<Type, IFactoryWithParameter> createFactory )
		{
			this.createFactory = createFactory;
		}

		protected override Func<object, object> CreateItem( Type parameter ) => o =>
		{
			var result = createFactory( parameter )?.Create( o );
			return result;
		};
	}

	public class FactoryWithParameterContainedDelegateFactory : FactoryBase<Type, Func<object>>
	{
		public static FactoryWithParameterContainedDelegateFactory Instance { get; } = new FactoryWithParameterContainedDelegateFactory();

		readonly Func<Type, Func<object, object>> factory;
		readonly Func<Type, object> createParameter;

		public FactoryWithParameterContainedDelegateFactory() : this( FactoryWithParameterDelegateFactory.Instance.Create ) {}

		public FactoryWithParameterContainedDelegateFactory( [Required]Func<Type, Func<object, object>> factory ) : this( factory, ActivateFactory<object>.Instance.CreateUsing )
		{}

		public FactoryWithParameterContainedDelegateFactory( [Required]Func<Type, Func<object, object>> factory, [Required]Func<Type, object> createParameter )
		{
			this.factory = factory;
			this.createParameter = createParameter;
		}

		protected override Func<object> CreateItem( Type parameter ) => () =>
		{
			var item = createParameter( Factory.GetParameterType( parameter ) );
			var @delegate = factory( parameter );
			var result = @delegate( item );
			return result;
		};
	}

	public class FuncFactory<T, U> : FactoryBase<Func<object, object>, Func<T, U>>
	{
		public static FuncFactory<T, U> Instance { get; } = new FuncFactory<T, U>();

		public FuncFactory() : base( new FactoryParameterCoercer<Func<object, object>>() ) { }

		protected override Func<T, U> CreateItem( Func<object, object> parameter ) => t => (U)parameter( t );
	}

	public class FuncFactory<T> : FactoryBase<Func<object>, Func<T>>
	{
		public static FuncFactory<T> Instance { get; } = new FuncFactory<T>();

		public FuncFactory() : base( new FactoryParameterCoercer<Func<object>>() ) {}

		protected override Func<T> CreateItem( Func<object> parameter ) => () => (T)parameter();
	}

	public abstract class RegisterFactoryCommandBase<TFactory> : Command<Type>
	{
		readonly IServiceRegistry registry;
		readonly Func<Type, Func<object>> create;

		protected RegisterFactoryCommandBase( [Required]IServiceRegistry registry, [Required]Func<Type, Func<object>> create )
		{
			this.registry = registry;
			this.create = create;
		}

		public override bool CanExecute( Type parameter ) => base.CanExecute( parameter ) && typeof(TFactory).Adapt().IsAssignableFrom( parameter );

		protected override void OnExecute( Type parameter )
		{
			var itemType = Factory.GetResultType( parameter );
			var func = create( parameter );
			registry.RegisterFactory( itemType, func );

			SingletonLocator.Instance.Locate( MakeGenericType( parameter, itemType ) ).AsValid<IFactoryWithParameter>( factory =>
			{
				var typed = Create( factory, parameter, func );
				registry.Register( typed.GetType(), typed );
			} );
		}

		protected virtual object Create( IFactoryWithParameter factory, Type type, Func<object> func ) => factory.Create( func );

		protected abstract Type MakeGenericType( Type parameter, Type itemType );
	}

	public class RegisterFactoryCommand : RegisterFactoryCommandBase<IFactory>
	{
		public RegisterFactoryCommand( IServiceRegistry registry ) : base( registry, FactoryDelegateFactory.Instance.Create ) {}

		public RegisterFactoryCommand( IServiceRegistry registry, Func<Type, Func<object>> create ) : base( registry, create ) {}

		protected override Type MakeGenericType( Type parameter, Type itemType ) => typeof(FuncFactory<>).MakeGenericType( itemType );
	}

	public class RegisterFactoryWithParameterCommand : RegisterFactoryCommandBase<IFactoryWithParameter>
	{
		public RegisterFactoryWithParameterCommand( IServiceRegistry registry ) : base( registry, FactoryWithParameterContainedDelegateFactory.Instance.Create ) {}

		public RegisterFactoryWithParameterCommand( IServiceRegistry registry, Func<Type, Func<object>> create ) : base( registry, create ) {}

		protected override Type MakeGenericType( Type parameter, Type itemType ) => typeof(FuncFactory<,>).MakeGenericType( Factory.GetParameterType( parameter ), itemType );

		protected override object Create( IFactoryWithParameter factory, Type type, Func<object> func ) => FactoryWithParameterDelegateFactory.Instance.Create( type );
	}
}
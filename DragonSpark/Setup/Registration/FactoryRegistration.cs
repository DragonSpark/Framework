using DragonSpark.Activation;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using Microsoft.Practices.Unity;
using PostSharp.Patterns.Contracts;
using System;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Setup.Registration
{
	public class FactoryRegistration : IRegistration
	{
		public static FactoryRegistration Instance { get; } = new FactoryRegistration();

		readonly Func<IActivator> activator;
		readonly Func<Type, Type> map;

		FactoryRegistration() : this( t => t ) {}

		protected FactoryRegistration( [Required]Func<Type, Type> map ) : this( Activator.GetCurrent, map ) {}

		protected FactoryRegistration( [Required]Func<IActivator> activator, [Required]Func<Type, Type> map )
		{
			this.activator = activator;
			this.map = map;
		}

		public void Register( IServiceRegistry registry, Type subject )
		{
			var factoryType = map( subject );
			var resultType = FactoryReflectionSupport.GetResultType( factoryType );
			var parameterType = FactoryReflectionSupport.Instance.GetParameterType( factoryType );
			var type = parameterType.With( t => typeof(RegisterFactoryCommand<,>).MakeGenericType( t, resultType ) ) ?? typeof(RegisterFactoryCommand<>).MakeGenericType( resultType );
			var command = activator().Construct<ICommand<Type>>( type, registry );
			command.Apply( factoryType );
		}
	}

	public static class ServiceRegistryExtensions
	{
		public static void Register<TFrom, TTo>( this IServiceRegistry @this, string name = null ) => @this.Register( typeof(TFrom), typeof(TTo), name );

		public static void Register<TService>( this IServiceRegistry @this, TService instance ) => @this.Register( typeof(TService), instance );

		public static void Register<TService>( this IServiceRegistry @this, Func<TService> factory ) => @this.RegisterFactory( typeof(TService), () => factory() );

		public static IServiceRegistry RegisterFactory<T>( [Required] this IServiceRegistry @this, [Required]IFactory<T> factory )
		{
			new RegisterFactoryCommand<T>( @this, type => factory.ToDelegate() ).Apply( factory.GetType() );
			return @this;
		}
	}

	public class FactoryDelegateFactory<T> : FactoryBase<Type, Func<T>>
	{
		readonly Func<Type, Func<T>> factory;

		public static FactoryDelegateFactory<T> Instance { get; } = new FactoryDelegateFactory<T>();

		public FactoryDelegateFactory() : this( t => ActivateFactory<IFactory<T>>.Instance.CreateUsing( t ).Create ) {}

		public FactoryDelegateFactory( [Required]Func<Type, Func<T>> factory )
		{
			this.factory = factory;
		}

		protected override Func<T> CreateItem( Type parameter ) => new Lazy<Func<T>>( () => factory( parameter ) ).Value;
	}

	public class FactoryDelegateFactory<T, U> : FactoryBase<Type, Func<T, U>>
	{
		public static FactoryDelegateFactory<T, U> Instance { get; } = new FactoryDelegateFactory<T, U>();

		protected override Func<T, U> CreateItem( Type parameter ) => new Lazy<Func<T, U>>( () => ActivateFactory<IFactory<T, U>>.Instance.CreateUsing( parameter ).Create ).Value;
	}

	public abstract class RegisterFactoryCommandBase<TFactory> : Command<Type>
	{
		readonly IServiceRegistry registry;
		readonly Func<Type, Delegate> factory;

		protected RegisterFactoryCommandBase( [Required]IServiceRegistry registry, [Required]Func<Type, Delegate> factory )
		{
			this.registry = registry;
			this.factory = factory;
		}

		public override bool CanExecute( Type parameter ) => base.CanExecute( parameter ) && parameter.Adapt().IsGenericOf<TFactory>();

		protected override void OnExecute( Type parameter )
		{
			var itemType = FactoryReflectionSupport.GetResultType( parameter );

			var func = factory( parameter );
			registry.RegisterFactory( itemType, () => func.DynamicInvoke() );
			registry.Register( func.GetType(), func );
		}
	}

	public class RegisterFactoryCommand<T> : RegisterFactoryCommandBase<IFactory<object>>
	{
		[InjectionConstructor]
		public RegisterFactoryCommand( IServiceRegistry registry ) : this( registry, FactoryDelegateFactory<T>.Instance.Create ) {}

		public RegisterFactoryCommand( IServiceRegistry registry, Func<Type, Delegate> factory ) : base( registry, factory )
		{}
	}

	public class RegisterFactoryCommand<T,U> : RegisterFactoryCommandBase<IFactory<object, object>>
	{
		public RegisterFactoryCommand( IServiceRegistry registry ) : base( registry, FactoryDelegateFactory<T, U>.Instance.Create ) { }
	}
}
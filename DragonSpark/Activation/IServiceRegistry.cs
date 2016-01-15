using DragonSpark.Activation.FactoryModel;
using DragonSpark.Activation.IoC;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Runtime.Specifications;
using Microsoft.Practices.Unity;
using PostSharp.Patterns.Contracts;
using System;

namespace DragonSpark.Activation
{
	public interface IServiceRegistry
	{
		void Register( MappingRegistrationParameter parameter );

		void Register( InstanceRegistrationParameter parameter );

		void RegisterFactory( FactoryRegistrationParameter parameter );
	}

	public class Always : WrappedSpecification<Type>
	{
		public Always() : base( AlwaysSpecification.Instance ) { }
	}

	public class OnlyIfNotRegistered : WrappedSpecification<Type>
	{
		public OnlyIfNotRegistered( IUnityContainer container ) : base( new InverseSpecification( new IsRegisteredSpecification( container ) ) ) { }
	}

	public class RegisterInstanceByConventionCommand : RegisterInstanceByConventionCommand<Always>
	{
		public RegisterInstanceByConventionCommand( IServiceRegistry registry, ImplementedFromConventionTypeLocator locator, Always specification ) : base( registry, locator, specification ) {}
	}

	public class RegisterInstanceByConventionCommand<T> : RegisterInstanceCommand<T> where T : ISpecification<Type>
	{
		readonly ImplementedFromConventionTypeLocator locator;

		public RegisterInstanceByConventionCommand( IServiceRegistry registry, [Required]ImplementedFromConventionTypeLocator locator, T specification ) : base( registry, specification )
		{
			this.locator = locator;
		}

		protected override void OnExecute( InstanceRegistrationParameter parameter ) => locator.Create( parameter.Instance.GetType() ).With( type =>
		{
			base.OnExecute( new InstanceRegistrationParameter( type, parameter.Instance, parameter.Name ) );
		} );
	}

	public class RegisterAllClassesCommand : RegisterAllClassesCommand<Always>
	{
		public RegisterAllClassesCommand( IServiceRegistry registry, Always specification ) : base( registry, specification ) {}
	}

	public class RegisterAllClassesCommand<T> : RegisterInstanceCommand<T> where T : ISpecification<Type>
	{
		public RegisterAllClassesCommand( IServiceRegistry registry, T specification ) : base( registry, specification ) {}

		protected override void OnExecute( InstanceRegistrationParameter parameter ) => parameter.Instance.Adapt().GetEntireHierarchy().Each( type =>
		{
			base.OnExecute( new InstanceRegistrationParameter( type, parameter.Instance, parameter.Name ) );
		} );
	}

	public abstract class RegistrationCommandBase<T, U> : Command<T> where T : RegistrationParameter where U : ISpecification<Type>
	{
		readonly Action<T> command;
		readonly U specification;

		protected RegistrationCommandBase( [Required]Action<T> command, [Required]U specification )
		{
			this.command = command;
			this.specification = specification;
		}

		public override bool CanExecute( T parameter ) => base.CanExecute( parameter ) && specification.IsSatisfiedBy( parameter.Type );

		protected override void OnExecute( T parameter ) => command( parameter );
	}

	public class RegisterCommand : RegisterCommand<Always>
	{
		public RegisterCommand( IServiceRegistry registry, Always specification ) : base( registry, specification ) {}
	}

	public class RegisterCommand<T> : RegistrationCommandBase<MappingRegistrationParameter, T> where T : ISpecification<Type>
	{
		public RegisterCommand( [Required]IServiceRegistry registry, T specification ) : base( registry.Register, specification ) {}
	}

	public class RegisterInstanceCommand : RegisterInstanceCommand<Always>
	{
		public RegisterInstanceCommand( IServiceRegistry registry, Always specification ) : base( registry, specification ) {}
	}

	public class RegisterInstanceCommand<T> : RegistrationCommandBase<InstanceRegistrationParameter, T> where T : ISpecification<Type>
	{
		public RegisterInstanceCommand( [Required]IServiceRegistry registry, T specification ) : base( registry.Register, specification ) { }
	}

	public class RegisterFactoryCommand : RegisterFactoryCommand<Always>
	{
		public RegisterFactoryCommand( IServiceRegistry registry, Always specification ) : base( registry, specification ) {}
	}

	public class RegisterFactoryCommand<T> : RegistrationCommandBase<FactoryRegistrationParameter, T> where T : ISpecification<Type>
	{
		public RegisterFactoryCommand( [Required]IServiceRegistry registry, T specification ) : base( registry.RegisterFactory, specification ) { }
	}
	
	public abstract class RegistrationParameter : ActivateParameter
	{
		protected RegistrationParameter( Type type, string name = null ) : base( type, name ) {}
	}

	public class MappingRegistrationParameter : RegistrationParameter
	{
		public MappingRegistrationParameter( Type type, string name = null ) : this( type, type, name ) {}

		public MappingRegistrationParameter( Type type, [Required]Type mappedTo, string name = null ) : base( type, name )
		{
			MappedTo = mappedTo;
		}

		public Type MappedTo { get; }
	}

	public class InstanceRegistrationParameter<T> : InstanceRegistrationParameter
	{
		public InstanceRegistrationParameter( T instance, string name = null ) : base( typeof(T), instance, name ) {}
	}

	public class InstanceRegistrationParameter : RegistrationParameter
	{
		public InstanceRegistrationParameter( [Required]object instance, string name = null ) : this( instance.GetType(), instance, name )
		{}

		public InstanceRegistrationParameter( Type type, [Required]object instance, string name = null ) : base( type, name )
		{
			Instance = instance;
		}

		public object Instance { get; }
	}

	public class FactoryRegistrationParameter : RegistrationParameter
	{
		public FactoryRegistrationParameter( Type type, [System.ComponentModel.DataAnnotations.Required]Func<object> factory, string name = null ) : base( type, name )
		{
			Factory = factory;
		}

		public Func<object> Factory { get; }
	}
}
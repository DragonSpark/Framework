using DragonSpark.Activation.FactoryModel;
using DragonSpark.Activation.IoC;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Runtime.Specifications;
using PostSharp.Patterns.Contracts;
using System;
using Microsoft.Practices.Unity;

namespace DragonSpark.Activation
{
	public interface IServiceRegistry
	{
		void Register( MappingRegistrationParameter parameter );

		void Register( InstanceRegistrationParameter parameter );

		void RegisterFactory( FactoryRegistrationParameter parameter );
	}

	public static class Specifications
	{
		public static ISpecification<Type> Default = AlwaysSpecification.Instance.Wrap<Type>();

		public static Func<IUnityContainer, ISpecification<Type>> NotRegistered { get; } = container => new InverseSpecification( new IsRegisteredSpecification( container ) ).Wrap<Type>();
	}

	public class RegisterInstanceByConventionCommand : RegisterInstanceCommand
	{
		readonly ImplementedFromConventionTypeLocator locator;

		public RegisterInstanceByConventionCommand( IServiceRegistry registry, ImplementedFromConventionTypeLocator locator ) : this( registry, locator, Specifications.Default ) {}

		public RegisterInstanceByConventionCommand( IServiceRegistry registry, [Required]ImplementedFromConventionTypeLocator locator, ISpecification<Type> specification ) : base( registry, specification )
		{
			this.locator = locator;
		}

		protected override void OnExecute( InstanceRegistrationParameter parameter ) => locator.Create( parameter.Instance.GetType() ).With( type =>
		{
			base.OnExecute( new InstanceRegistrationParameter( type, parameter.Instance, parameter.Name ) );
		} );
	}

	public class RegisterAllClassesCommand : RegisterInstanceCommand
	{
		public RegisterAllClassesCommand( IServiceRegistry registry ) : base( registry ) {}

		public RegisterAllClassesCommand( IServiceRegistry registry, ISpecification<Type> specification ) : base( registry, specification ) {}

		protected override void OnExecute( InstanceRegistrationParameter parameter ) => parameter.Instance.Adapt().GetEntireHierarchy().Each( type =>
		{
			base.OnExecute( new InstanceRegistrationParameter( type, parameter.Instance, parameter.Name ) );
		} );
	}

	public abstract class RegistrationCommandBase<T> : Command<T> where T : RegistrationParameter
	{
		readonly Action<T> command;
		readonly ISpecification<Type> specification;

		protected RegistrationCommandBase( [Required]Action<T> command ) : this( command, Specifications.Default ) {}

		protected RegistrationCommandBase( [Required]Action<T> command, [Required]ISpecification<Type> specification )
		{
			this.command = command;
			this.specification = specification;
		}

		public override bool CanExecute( T parameter ) => base.CanExecute( parameter ) && specification.IsSatisfiedBy( parameter.Type );

		protected override void OnExecute( T parameter ) => command( parameter );
	}

	public class RegisterCommand : RegistrationCommandBase<MappingRegistrationParameter>
	{
		public RegisterCommand( IServiceRegistry registry ) : this( registry, Specifications.Default ) {}

		public RegisterCommand( [Required]IServiceRegistry registry, ISpecification<Type> specification ) : base( registry.Register, specification ) {}
	}

	public class RegisterInstanceCommand : RegistrationCommandBase<InstanceRegistrationParameter>
	{
		public RegisterInstanceCommand( IServiceRegistry registry ) : this( registry, Specifications.Default ) { }

		public RegisterInstanceCommand( [Required]IServiceRegistry registry, ISpecification<Type> specification ) : base( registry.Register, specification ) { }
	}

	public class RegisterFactoryCommand : RegistrationCommandBase<FactoryRegistrationParameter>
	{
		public RegisterFactoryCommand( IServiceRegistry registry ) : this( registry, Specifications.Default ) { }

		public RegisterFactoryCommand( [Required]IServiceRegistry registry, ISpecification<Type> specification ) : base( registry.RegisterFactory, specification ) { }
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
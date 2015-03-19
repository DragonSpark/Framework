using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DragonSpark.Extensions;
using Microsoft.Practices.Unity;
using Prism;
using Prism.Unity;

namespace DragonSpark.Setup
{
	[AttributeUsage( AttributeTargets.Class )]
	public sealed class RegisterAsAttribute : Attribute
	{
		readonly Type @as;

		public RegisterAsAttribute( Type @as )
		{
			this.@as = @as;
		}

		public Type As
		{
			get { return @as; }
		}
	}

	public class RegistrationContext
	{
		readonly IUnityContainer container;
		readonly Assembly[] application;
		readonly Assembly[] include;
		readonly TypeInfo[] candidates;

		public RegistrationContext( IUnityContainer container, Assembly[] application, Assembly[] include, TypeInfo[] candidates )
		{
			this.container = container;
			this.application = application;
			this.include = include;
			this.candidates = candidates;
		}

		public IUnityContainer Container
		{
			get { return container; }
		}

		public ICollection<Assembly> Application
		{
			get { return application; }
		}

		public ICollection<Assembly> Include
		{
			get { return include; }
		}

		public ICollection<TypeInfo> Candidates
		{
			get { return candidates; }
		}
	}

	/*public interface IConventionRegister
	{
		void 
	}*/

	public abstract class ApplyRegistrationsCommand : SetupCommand
	{
		public virtual IAssemblyLocator Locator { get; set; }

		protected override void Execute( SetupContext context )
		{
			var assemblies = DetermineAssemblies();
			
			var types = DetermineCandidateTypes( assemblies );

			var registrationContext = new RegistrationContext( context.Container(), assemblies, assemblies.SelectMany( assembly => assembly.FromMetadata<IncludeAttribute, IEnumerable<Assembly>>( attribute => attribute.Assemblies ) ).ToArray(), types );

			RegisterBasedOnConvention( registrationContext );

			RegisterBasedOnMetadata( registrationContext );
		}

		protected virtual Assembly[] DetermineAssemblies()
		{
			var assemblies = Locator.GetApplicationAssemblies().ToArray();
			return assemblies
				.OrderBy( x => x.FromMetadata<RegistrationAttribute, Priority>( z => z.Priority, () => Priority.Normal ) ).ToArray();
		}

		protected virtual void RegisterBasedOnMetadata( RegistrationContext context )
		{
			context.Candidates.WhereDecorated<RegisterAsAttribute>().Apply( item => item.Item2.AsType().With( type =>
			{
				Debug.WriteLine( "Registering {0} -> {1}", item.Item1.As, type );
				context.Container.RegisterType( item.Item1.As, type, DetermineLifetimeContainer<ContainerControlledLifetimeManager>( type ) );
			} ) );
		}

		protected abstract void RegisterBasedOnConvention( RegistrationContext context );

		protected virtual TypeInfo[] DetermineCandidateTypes( Assembly[] assemblies )
		{
			var result = assemblies
				.SelectMany( assembly => assembly.DefinedTypes.Except( assembly.FromMetadata<RegistrationAttribute, IEnumerable<TypeInfo>>( attribute => attribute.IgnoreForRegistration.AsTypeInfos() ) ) ).ToArray();
			return result;
		}

		protected virtual LifetimeManager DetermineLifetimeContainer<T>( Type type ) where T : LifetimeManager, new()
		{
			var result = type.FromMetadata<LifetimeManagerAttribute, LifetimeManager>( x => Activation.Activator.CreateInstance<LifetimeManager>( x.LifetimeManagerType ) ) ?? new T();
			return result;
		}
	}
}
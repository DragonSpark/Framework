using DragonSpark.Extensions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Activation.IoC.Commands
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

	public abstract class ApplyRegistrationsCommand : IContainerConfigurationCommand
	{
		/*public string AssemblyNamesStartsWith { get; set; }

		public string IgnoreNamespaces { get; set; }

		public Collection<Type> IgnoreNamespacesFromTypes
		{
			get { return ignoreNamespacesFromTypes; }
		}	readonly Collection<Type> ignoreNamespacesFromTypes = new Collection<Type>();

		protected virtual IEnumerable<Type> IgnorableTypes
		{
			get { return new[] { typeof(PartialApply), typeof(IUnityContainerTypeConfiguration) }; }
		}*/

		public virtual IAssemblyLocator Locator { get; set; }

		public void Configure( IUnityContainer container )
		{
			var assemblies = DetermineAssemblies();
			
			var types = DetermineCandidateTypes( assemblies );

			var context = new RegistrationContext( container, assemblies, assemblies.SelectMany( assembly => assembly.FromMetadata<IncludeAttribute, IEnumerable<Assembly>>( attribute => attribute.Assemblies ) ).ToArray(), types );

			RegisterBasedOnConvention( context );

			RegisterBasedOnMetadata( context );
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

		/*protected virtual Type[] DetermineTypesForConvention()
		{
			/*var ignore = string.Join( ";", IgnoreNamespaces.Prepend( IgnorableTypes.Concat( IgnoreNamespacesFromTypes ).Select( x => x.Namespace ).ToArray() ) ).ToStringArray();
			var names = AssemblyNamesStartsWith.ToStringArray();
			var namespaces = ignore.Concat( Locator.GetAllAssemblies().SelectMany( x => x.GetCustomAttributes<IgnoreNamespaceDuringRegistrationAttribute>().Select( y => y.Namespace ) ) ).ToArray();#1#
			var types = DetermineCandidateTypes()
				/*.Where( x => !names.Any() || names.Any( y => x.GetTypeInfo().Assembly.GetName().Name.StartsWith( y, StringComparison.CurrentCultureIgnoreCase ) ) )
				.Where( x => namespaces.All( y => !x.Namespace.StartsWith( y, StringComparison.CurrentCultureIgnoreCase ) ) )#1#
				// .Where( AdditionalFilter )
				.ToArray();
			return types;
		}*/

		protected abstract void RegisterBasedOnConvention( RegistrationContext context );

		/*protected virtual Func<Type, bool> AdditionalFilter
		{
			get { return x => true; }
		}*/

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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using DragonSpark.Extensions;
using Dynamitey;
using Microsoft.Practices.Unity;

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

	public abstract class ApplyRegistrationsCommand : IContainerConfigurationCommand
	{
		public string AssemblyNamesStartsWith { get; set; }

		public string IgnoreNamespaces { get; set; }

		public Collection<Type> IgnoreNamespacesFromTypes
		{
			get { return ignoreNamespacesFromTypes; }
		}	readonly Collection<Type> ignoreNamespacesFromTypes = new Collection<Type>();

		protected virtual IEnumerable<Type> IgnorableTypes
		{
			get { return new[] { typeof(PartialApply), typeof(IUnityContainerTypeConfiguration) }; }
		}

		public virtual IAssemblyLocator Locator { get; set; }

		public void Configure( IUnityContainer container )
		{
			var assemblies = Locator.GetAllAssemblies().ToArray();
			var types = DetermineTypesForConvention( assemblies );
			RegisterFromConvention( container, types );

			RegisterFromMetadata( container );

		}

		protected virtual void RegisterFromMetadata( IUnityContainer container )
		{
			Locator.GetAllTypesWith<RegisterAsAttribute>().Apply( item => item.Item2.AsType().With( type =>
			{
				container.RegisterType( item.Item1.As, type, DetermineLifetimeContainer<ContainerControlledLifetimeManager>( type ) );
			} ) );
		}

		protected virtual Type[] DetermineTypesForConvention( Assembly[] assemblies )
		{
			var ignore = string.Join( ";", IgnoreNamespaces.Prepend( IgnorableTypes.Concat( IgnoreNamespacesFromTypes ).Select( x => x.Namespace ).ToArray() ) ).ToStringArray();
			var names = AssemblyNamesStartsWith.ToStringArray();
			var namespaces = ignore.Concat( Locator.GetAllAssemblies().SelectMany( x => x.GetCustomAttributes<IgnoreNamespaceDuringRegistrationAttribute>().Select( y => y.Namespace ) ) ).ToArray();
			var types = DetermineCandidateTypes()
				.Where( x => !names.Any() || names.Any( y => x.GetTypeInfo().Assembly.GetName().Name.StartsWith( y, StringComparison.CurrentCultureIgnoreCase ) ) )
				.Where( x => namespaces.All( y => !x.Namespace.StartsWith( y, StringComparison.CurrentCultureIgnoreCase ) ) )
				.Where( AdditionalFilter )
				.OrderBy( x => x.GetTypeInfo().Assembly.FromMetadata<RegistrationAttribute, Priority>( z => z.Priority, () => Priority.Normal ) ).ToArray();
			return types;
		}

		protected abstract void RegisterFromConvention( IUnityContainer container, Type[] types );

		protected virtual Func<Type, bool> AdditionalFilter
		{
			get { return x => true; }
		}

		protected abstract IEnumerable<Type> DetermineCandidateTypes();

		protected virtual LifetimeManager DetermineLifetimeContainer<T>( Type type ) where T : LifetimeManager, new()
		{
			var result = type.FromMetadata<LifetimeManagerAttribute, LifetimeManager>( x => Activation.Activator.CreateInstance<LifetimeManager>( x.LifetimeManagerType ) ) ?? new T();
			return result;
		}
	}
}
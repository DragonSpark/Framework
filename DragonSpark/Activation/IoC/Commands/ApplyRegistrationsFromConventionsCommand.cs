using DragonSpark.Extensions;
using Dynamitey;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Activation.IoC.Commands
{
	public abstract class ApplyRegistrationsFromConventionsCommand : IContainerConfigurationCommand
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

		public IAssemblyLocator Locator { get; set; }

		public void Configure( IUnityContainer container )
		{
			var ignore = string.Join( ";", IgnoreNamespaces.AsItem( IgnorableTypes.Concat( IgnoreNamespacesFromTypes ).Select( x => x.Namespace ).ToArray() ) ).ToStringArray();
			var names = AssemblyNamesStartsWith.ToStringArray();
			var namespaces = ignore.Concat( Locator.GetAllAssemblies().SelectMany( x => x.GetCustomAttributes<IgnoreNamespaceDuringRegistrationAttribute>().Select( y => y.Namespace ) ) ).ToArray();
			Register( container, names, namespaces );
		}

		protected abstract void Register( IUnityContainer container, string[] names, string[] namespaces );

		protected virtual LifetimeManager DetermineLifetimeContainer( Type type )
		{
			var result = type.FromMetadata<LifetimeManagerAttribute, LifetimeManager>( x => Activation.Activator.CreateInstance<LifetimeManager>( x.LifetimeManagerType ) ) ?? new ContainerControlledLifetimeManager();
			return result;
		}
	}
}
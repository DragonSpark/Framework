using AutoMapper;
using DragonSpark.Activation.IoC.Commands;
using DragonSpark.Extensions;
using Dynamitey;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Markup;

namespace DragonSpark.Common.IoC.Commands
{
	[ContentProperty( "IgnoreNamespacesFromTypes" )]
	public class ApplyRegistrationsFromConventionsCommand : Activation.IoC.Commands.ApplyRegistrationsFromConventionsCommand
	{
		public ApplyRegistrationsFromConventionsCommand()
		{
			IgnoreNamespaces = string.Join( ";", new[] { typeof(PartialApply), typeof(Notification), typeof(EventAggregator), typeof(Region), typeof(Mapper), typeof(ModuleCatalog), typeof(IUnityContainerTypeConfiguration) }.Select( x => x.Namespace ).ToArray() );
		}

		protected override IEnumerable<Type> IgnorableTypes
		{
			get { return base.IgnorableTypes.Concat( new [] { typeof(Notification), typeof(EventAggregator), typeof(Region), typeof(Mapper), typeof(ModuleCatalog) } ); }
		}

		protected override void Register( IUnityContainer container, string[] names, string[] namespaces )
		{
			var types = AllClasses.FromAssembliesInBasePath().Where( x => x.Namespace != null )
				.Where( x => !names.Any() || names.Any( y => x.Assembly.GetName().Name.StartsWith( y, StringComparison.InvariantCultureIgnoreCase ) ) )
				.Where( x => namespaces.All( y => !x.Namespace.StartsWith( y, StringComparison.InvariantCultureIgnoreCase ) ) )
				.Where( x => WithMappings.FromMatchingInterface( x ).Any( y => y.IsPublic ) )
				.OrderBy( x => x.Assembly.FromMetadata<RegistrationAttribute, Priority>( z => z.Priority, () => Priority.Normal ) ).ToArray();

			container.RegisterTypes( types, WithMappings.FromMatchingInterface, WithName.Default, DetermineLifetimeContainer, overwriteExistingMappings: true );
		}
	}
}
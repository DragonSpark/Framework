using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Markup;
using AutoMapper;
using DragonSpark.Application.Runtime;
using DragonSpark.ComponentModel;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace DragonSpark.Application.IoC.Commands
{

	[ContentProperty( "IgnoreNamespacesFromTypes" )]
	public class ApplyRegistrationsCommand : Activation.IoC.Commands.ApplyRegistrationsCommand
	{
		protected override IEnumerable<Type> IgnorableTypes
		{
			get { return base.IgnorableTypes.Concat( new [] { typeof(Notification), typeof(EventAggregator), typeof(Region), typeof(Mapper), typeof(ModuleCatalog) } ); }
		}

		[Activate( typeof(AssemblyLocator) )]
		public override IAssemblyLocator Locator
		{
			get { return base.Locator; }
			set { base.Locator = value; }
		}

		protected override void RegisterFromConvention( IUnityContainer container, Type[] types )
		{
			container.RegisterTypes( types, WithMappings.FromMatchingInterface, WithName.Default, DetermineLifetimeContainer<ContainerControlledLifetimeManager>, overwriteExistingMappings: true );
		}

		protected override Func<Type, bool> AdditionalFilter
		{
			get { return x => WithMappings.FromMatchingInterface( x ).Any( y => y.IsPublic ); }
		}

		protected override IEnumerable<Type> DetermineCandidateTypes()
		{
			var result = AllClasses.FromAssembliesInBasePath().Where( x => x.Namespace != null );
			return result;
		}
	}
}
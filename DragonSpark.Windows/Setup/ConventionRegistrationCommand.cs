using DragonSpark.Activation.FactoryModel;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Setup.Registration;
using Microsoft.Practices.Unity;
using PostSharp.Patterns.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Windows.Setup
{
	public class ConventionRegistrationCommand : Command<ConventionRegistrationProfile>
	{
		readonly IUnityContainer container;
		readonly IMessageLogger messageLogger;
		readonly IFactory<ActivateParameter, LifetimeManager> lifetimeFactory;

		public ConventionRegistrationCommand( [Required]IUnityContainer container, [Required]IMessageLogger messageLogger, [Required]IFactory<ActivateParameter, LifetimeManager> lifetimeFactory )
		{
			this.container = container;
			this.messageLogger = messageLogger;
			this.lifetimeFactory = lifetimeFactory;
		}

		protected override void OnExecute( ConventionRegistrationProfile parameter )
		{
			var ignore = parameter.Application.SelectMany( assembly => AttributeProviderExtensions.From<RegistrationAttribute, IEnumerable<Type>>( assembly, attribute => attribute.IgnoreForRegistration.Concat( Ignore( attribute, assembly ) ) ) ).ToArray();
			var register = parameter.Candidates.Except( ignore )
				.Where( x => WithMappings.FromMatchingInterface( x ).Any( found => ( found.IsPublic || found.IsNestedPublic ) && !ignore.Contains( found ) && !container.IsRegistered( found ) ) )
				.ToArray();
			register.Each( type => messageLogger.Information( $"Registering from convention: {type.FullName}" ) );
			container.RegisterTypes( register.Reverse(), WithMappings.FromMatchingInterface, WithName.Default, lifetimeFactory.CreateUsing, overwriteExistingMappings: true );
		}

		static IEnumerable<TypeInfo> Ignore( RegistrationAttribute attribute, Assembly assembly ) => attribute.Namespaces.With( s => s.ToStringArray().With( ns => assembly.DefinedTypes.Where( type => ns.Contains( type.Namespace ) ) ) );
	}
}
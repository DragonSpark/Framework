using System;
using System.Collections.Generic;
using System.Linq;
using DragonSpark.Extensions;
using DragonSpark.Logging;
using DragonSpark.Setup;
using Microsoft.Practices.Unity;

namespace DragonSpark.Windows.Setup
{
	public class UnityConventionRegistrationService : DragonSpark.Setup.UnityConventionRegistrationService
	{
		public UnityConventionRegistrationService( IUnityContainer container, ILoggerFacade logger, ISingletonLocator locator ) : base( container, logger, locator )
		{}

		public override void Register( ConventionRegistrationProfile profile )
		{
			RegisterTypes( profile );

			base.Register( profile );
		}

		protected virtual void RegisterTypes( ConventionRegistrationProfile profile )
		{
			var ignore = profile.Application.SelectMany( assembly => assembly.FromMetadata<RegistrationAttribute, IEnumerable<Type>>( attribute => attribute.IgnoreForRegistration.Concat( attribute.Namespaces.Transform( s => s.ToStringArray().Transform( ns => assembly.ExportedTypes.Where( type => ns.Contains( type.Namespace ) ) ) ) ) ) ).ToArray();
			var register = profile.Candidates
				.Where( x => WithMappings.FromMatchingInterface( x ).Any( found => found.IsPublic && !ignore.Contains( found ) && !Container.IsRegistered( found ) ) )
				.ToArray();
			register.Apply( type => Logger.Log( $"Registering from convention: {type.FullName}", Category.Debug, DragonSpark.Logging.Priority.None ) );
			Container.RegisterTypes( register, WithMappings.FromMatchingInterface, WithName.Default, DetermineLifetimeContainer<ContainerControlledLifetimeManager>, overwriteExistingMappings: true );
		}
	}
}
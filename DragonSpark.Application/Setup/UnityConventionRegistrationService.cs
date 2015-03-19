using DragonSpark.Extensions;
using DragonSpark.Setup;
using Microsoft.Practices.Unity;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Application.Setup
{
	public class UnityConventionRegistrationService : DragonSpark.Setup.UnityConventionRegistrationService
	{
		public UnityConventionRegistrationService( IUnityContainer container, ILoggerFacade logger ) : base( container, logger )
		{}

		public override void Register( ConventionRegistrationProfile profile )
		{
			RegisterTypes( profile );

			base.Register( profile );
		}

		protected virtual void RegisterTypes( ConventionRegistrationProfile profile )
		{
			var ignore = profile.Application.SelectMany( assembly => assembly.FromMetadata<RegistrationAttribute, IEnumerable<Type>>( attribute => attribute.IgnoreForRegistration ) ).ToArray();
			var register = profile.Candidates.Where( x => WithMappings.FromMatchingInterface( x )
				.Any( found => found.IsPublic && !ignore.Contains( found ) )
				).ToArray();
			register.Apply( type => Logger.Log( string.Format( "Registering for convention: {0}", (object)type.FullName ), Category.Debug, Prism.Logging.Priority.None ) );
			Container.RegisterTypes( register, WithMappings.FromMatchingInterface, WithName.Default, DetermineLifetimeContainer<ContainerControlledLifetimeManager>, overwriteExistingMappings: true );
		}
	}
}
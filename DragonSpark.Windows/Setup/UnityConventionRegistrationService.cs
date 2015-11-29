using DragonSpark.Activation;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Setup;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using DragonSpark.Setup.Registration;

namespace DragonSpark.Windows.Setup
{
	public class UnityConventionRegistrationService : DragonSpark.Setup.Registration.UnityConventionRegistrationService
	{
		public UnityConventionRegistrationService( IUnityContainer container, ILogger logger ) : base( container, logger )
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
			register.Each( type => Logger.Information( $"Registering from convention: {type.FullName}" ) );
			Container.RegisterTypes( register, WithMappings.FromMatchingInterface, WithName.Default, Factory.CreateUsing, overwriteExistingMappings: true );
		}
	}
}
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Setup.Registration;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DragonSpark.Modularity;

namespace DragonSpark.Windows.Setup
{
	public class UnityConventionRegistrationService : DragonSpark.Setup.Registration.UnityConventionRegistrationService
	{
		public UnityConventionRegistrationService( IUnityContainer container, IMessageLogger messageLogger ) : base( container, messageLogger )
		{}

		public override void Register( ConventionRegistrationProfile profile )
		{
			base.Register( profile );
			RegisterTypes( profile );
		}

		protected virtual void RegisterTypes( ConventionRegistrationProfile profile )
		{
			var ignore = profile.Application.SelectMany( assembly => assembly.FromMetadata<RegistrationAttribute, IEnumerable<Type>>( attribute => attribute.IgnoreForRegistration.Concat( Ignore( attribute, assembly ) ) ) ).ToArray();
			var register = profile.Candidates.Except( ignore )
				.Where( x => WithMappings.FromMatchingInterface( x ).Any( found => ( found.IsPublic || found.IsNestedPublic ) && !ignore.Contains( found ) && !Container.IsRegistered( found ) ) )
				.ToArray();
			register.Each( type => MessageLogger.Information( $"Registering from convention: {type.FullName}" ) );
			Container.RegisterTypes( register.Reverse(), WithMappings.FromMatchingInterface, WithName.Default, Factory.CreateUsing, overwriteExistingMappings: true );
		}

		static IEnumerable<TypeInfo> Ignore( RegistrationAttribute attribute, Assembly assembly )
		{
			var result = attribute.Namespaces.With( s => s.ToStringArray().With( ns => assembly.DefinedTypes.Where( type => ns.Contains( type.Namespace ) ) ) );
			return result;
		}
	}
}
using DragonSpark.Activation.IoC;
using DragonSpark.Aspects;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using Microsoft.Practices.Unity;
using System;
using DragonSpark.Activation;

namespace DragonSpark.Testing.Framework.Setup.Location
{
	public interface IServiceLocationAuthority
	{
		void Register( Type item, bool enabled );

		bool IsAllowed( Type type );
	}

	public class FixtureExtension : UnityContainerExtension
	{
		[Value( typeof(CurrentAutoDataContext) )]
		public AutoData Setup { get; set; }

		[Activate]
		public PersistingServiceRegistry Registry { get; set; }

		[BuildUp]
		protected override void Initialize()
		{
			new RegisterInstanceCommand( Registry, Specifications.NotRegistered( Container ) ).ExecuteWith( new InstanceRegistrationParameter( Setup.Fixture ) );
			// Container.Extend().Policies.Insert( 0, new FixtureBuildPlanStrategy() );
		}
	}
}
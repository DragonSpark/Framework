using DragonSpark.Activation;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using Microsoft.Practices.Unity;
using Ploeh.AutoFixture;
using System;

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
		public RegisterInstanceCommand<OnlyIfNotRegistered> Command { get; set; }

		[Activate]
		public AuthorizedServiceLocationRelay Relay { get; set; }

		protected override void Initialize()
		{
			Command.ExecuteWith( new InstanceRegistrationParameter<IFixture>( Setup.Fixture ) );

			Setup.Fixture.ResidueCollectors.Add( Relay );
		}
	}
}
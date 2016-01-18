using DragonSpark.Activation;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using Microsoft.Practices.Unity;
using Ploeh.AutoFixture;
using System;
using PostSharp.Patterns.Contracts;

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

		[Required, Locate]
		public RegisterInstanceCommand<OnlyIfNotRegistered> Command { [return: Required]get; set; }

		[Required, Locate]
		public AuthorizedServiceLocationRelay Relay { [return: Required]get; set; }

		protected override void Initialize()
		{
			Command.ExecuteWith( new InstanceRegistrationParameter<IFixture>( Setup.Fixture ) );

			Setup.Fixture.ResidueCollectors.Add( Relay );
		}
	}
}
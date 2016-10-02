using DragonSpark.Application.Setup;
using DragonSpark.TypeSystem.Generics;
using Ploeh.AutoFixture;

namespace DragonSpark.Testing.Framework.Runtime
{
	sealed class FixtureRegistry : InstanceRepository
	{
		readonly IFixture fixture;

		readonly GenericMethodCommands commands;

		public FixtureRegistry( IFixture fixture )
		{
			this.fixture = fixture;
			commands = new GenericMethodCommands( this );
		}

		public override void Add( InstanceRegistrationRequest request )
		{
			commands[nameof(RegisterInstance)].Make( request.RegistrationType ).Invoke( request.Instance );
			base.Add( request );
		}

		void RegisterInstance<T>( T instance ) => fixture.Inject( instance );
	}
}
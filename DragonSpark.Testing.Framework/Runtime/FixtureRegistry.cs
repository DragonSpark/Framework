using DragonSpark.Application;
using DragonSpark.Application.Setup;
using DragonSpark.TypeSystem.Generics;
using Ploeh.AutoFixture;

namespace DragonSpark.Testing.Framework.Runtime
{
	sealed class FixtureRegistry : InstanceRepository
	{
		readonly IFixture fixture;

		readonly IGenericMethodContext<Execute> context;

		public FixtureRegistry( IFixture fixture )
		{
			this.fixture = fixture;
			context = new GenericMethodCommands( this )[nameof(RegisterInstance)];
		}

		public override void Add( ServiceRegistration request )
		{
			context.Make( request.RegistrationType ).Invoke( request.Instance );
			base.Add( request );
		}

		void RegisterInstance<T>( T instance ) => fixture.Inject( instance );
	}
}
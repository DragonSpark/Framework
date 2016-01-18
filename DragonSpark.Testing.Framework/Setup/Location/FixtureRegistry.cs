using System;
using DragonSpark.Activation;
using DragonSpark.Extensions;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

namespace DragonSpark.Testing.Framework.Setup.Location
{
	class FixtureRegistry : IServiceRegistry
	{
		readonly IFixture fixture;

		public FixtureRegistry( IFixture fixture )
		{
			this.fixture = fixture;
		}

		public void Register( MappingRegistrationParameter parameter ) => fixture.Customizations.Add( new TypeRelay( parameter.Type, parameter.MappedTo ) );

		public void Register( InstanceRegistrationParameter parameter ) => this.InvokeGenericAction( nameof(RegisterInstance), new[] { parameter.Type }, parameter.Instance );

		void RegisterInstance<T>( T instance ) => fixture.Inject( instance );

		public void RegisterFactory( FactoryRegistrationParameter parameter ) => this.InvokeGenericAction( nameof(RegisterFactory), parameter.Type.ToItem(), parameter.Factory );

		void RegisterFactory<T>( Func<object> factory ) => fixture.Customize<T>( c => c.FromFactory( () => (T)factory() ).OmitAutoProperties() );
	}
}
using DragonSpark.Activation;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using Microsoft.Practices.ServiceLocation;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using System;

namespace DragonSpark.Testing.Framework.Setup.Location
{
	public abstract class CustomizationBase : ICustomization
	{
		void ICustomization.Customize( IFixture fixture ) => Customize( fixture );

		protected abstract void Customize( IFixture fixture );
	}

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
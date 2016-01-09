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
		[Aspects.BuildUp]
		void ICustomization.Customize( IFixture fixture ) => Customize( fixture );

		protected abstract void Customize( IFixture fixture );
	}

	public class ServiceLocationCustomization : CustomizationBase
	{
		[Activate]
		public IServiceLocator Locator { get; set; }

		[Activate]
		public IServiceLocationAuthority Authority { get; set; }

		protected override void Customize( IFixture fixture ) => Locator.With( locator =>
		{
			new ServiceLocationRelay( locator, new AuthorizedLocationSpecification( locator, Authority ) ).With( fixture.ResidueCollectors.Add );
		} );
	}
	
	class FixtureRegistry : IServiceRegistry
	{
		readonly IFixture fixture;

		public FixtureRegistry( IFixture fixture )
		{
			this.fixture = fixture;
		}

		public void Register( Type @from, Type mappedTo, string name = null ) => fixture.Customizations.Add( new TypeRelay( @from, mappedTo ) );

		public void Register( Type type, object instance ) => this.InvokeGenericAction( nameof(RegisterInstance), new[] { type }, instance );

		void RegisterInstance<T>( T instance ) => fixture.Inject( instance );

		public void RegisterFactory( Type type, Func<object> factory ) => this.InvokeGenericAction( nameof(RegisterFactory), type.ToItem(), factory );

		void RegisterFactory<T>( Func<object> factory ) => fixture.Customize<T>( c => c.FromFactory( () => (T)factory() ).OmitAutoProperties() );
	}
}
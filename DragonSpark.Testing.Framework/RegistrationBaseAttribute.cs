using DragonSpark.Activation;
using DragonSpark.Aspects;
using DragonSpark.Runtime.Values;
using DragonSpark.Setup.Registration;
using DragonSpark.Testing.Framework.Setup.Location;
using DragonSpark.TypeSystem;
using Ploeh.AutoFixture;
using PostSharp.Patterns.Contracts;
using System;

namespace DragonSpark.Testing.Framework
{
	[AttributeUsage( AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = true )]
	public abstract class RegistrationBaseAttribute : HostingAttribute
	{
		protected RegistrationBaseAttribute( Func<ICustomization> factory ) : base( factory ) {}
	}

	public class RegistrationCustomization : ICustomization
	{
		readonly IRegistration registration;

		public RegistrationCustomization( [Required]IRegistration registration )
		{
			this.registration = registration;
		}

		public void Customize( IFixture fixture ) => registration.Register( new AssociatedRegistry( fixture ).Item, null );

		public class AssociatedRegistry : AssociatedValue<IFixture, IServiceRegistry>
		{
			public AssociatedRegistry( [Required]IFixture instance ) : base( instance, () => new FixtureRegistry( instance ) ) {}
		}
	}

	public class RegisterFactoryAttribute : RegistrationBaseAttribute
	{
		public RegisterFactoryAttribute( Type factoryType ) : base( () => new RegistrationCustomization( new FactoryRegistration( factoryType ) ) ) {}

		public class FactoryRegistration : DragonSpark.Setup.Registration.FactoryRegistration
		{
			public FactoryRegistration( [OfFactoryType] Type factoryType ) : base( t => factoryType ) { }
		}
	}
}
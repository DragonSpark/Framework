using DragonSpark.Activation;
using DragonSpark.Aspects;
using DragonSpark.Runtime.Values;
using DragonSpark.Setup.Registration;
using DragonSpark.Testing.Framework.Setup.Location;
using DragonSpark.TypeSystem;
using Ploeh.AutoFixture;
using PostSharp.Patterns.Contracts;
using System;
using DragonSpark.Extensions;

namespace DragonSpark.Testing.Framework
{
	[AttributeUsage( AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = true )]
	public abstract class RegistrationBaseAttribute : HostingAttribute
	{
		protected RegistrationBaseAttribute( Func<Type, ICustomization> factory ) : base( x => x.AsTo( factory ) ) {}
	}

	public class RegistrationCustomization : ICustomization
	{
		readonly IRegistration registration;

		public RegistrationCustomization( [Required]IRegistration registration )
		{
			this.registration = registration;
		}

		public void Customize( IFixture fixture ) => registration.Register( new AssociatedRegistry( fixture ).Item );

		public class AssociatedRegistry : AssociatedValue<IFixture, IServiceRegistry>
		{
			public AssociatedRegistry( [Required]IFixture instance ) : base( instance, () => new FixtureRegistry( instance ) ) {}
		}
	}

	public class RegisterFactoryAttribute : RegistrationBaseAttribute
	{
		public RegisterFactoryAttribute( Type factoryType ) : base( t => new RegistrationCustomization( new FactoryRegistration( factoryType ) ) ) {}
	}
}
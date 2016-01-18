using DragonSpark.Activation;
using DragonSpark.Extensions;
using DragonSpark.Runtime.Values;
using DragonSpark.Setup.Registration;
using DragonSpark.Testing.Framework.Setup.Location;
using DragonSpark.TypeSystem;
using Ploeh.AutoFixture;
using PostSharp.Patterns.Contracts;
using System;
using System.Reflection;
using Type = System.Type;

namespace DragonSpark.Testing.Framework
{
	[AttributeUsage( AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = true )]
	public abstract class RegistrationBaseAttribute : HostingAttribute
	{
		protected RegistrationBaseAttribute( Func<object, ICustomization> factory ) : base( x => x.AsTo( factory ) ) {}
	}

	[Priority( Priority.Low )]
	public class AssembliesAttribute : RegistrationBaseAttribute
	{
		public AssembliesAttribute() : base( o => AssignAssemblyHostCustomization.Instance ) {}
	}

	public class AssignAssemblyHostCustomization : ICustomization
	{
		public static AssignAssemblyHostCustomization Instance { get; } = new AssignAssemblyHostCustomization();

		public void Customize( IFixture fixture )
		{
			var assemblies = fixture.Create<Assembly[]>();
			new AssemblyHost().Assign( assemblies );
		}
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
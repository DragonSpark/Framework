using DragonSpark.Activation;
using Ploeh.AutoFixture;
using System;
using DragonSpark.Testing.Framework.Setup.Location;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Testing.Framework
{
	[AttributeUsage( AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = true )]
	public abstract class RegistrationAttribute : Attribute, ICustomization
	{
		protected RegistrationAttribute( Type registrationType ) : this( registrationType, registrationType )
		{}

		protected RegistrationAttribute( [Required]Type registrationType, [Required]Type mappedTo )
		{
			RegistrationType = registrationType;
			MappedTo = mappedTo;
		}

		protected Type RegistrationType { get; }

		protected Type MappedTo { get; }

		void ICustomization.Customize( IFixture fixture )
		{
			var registry = new FixtureRegistry( fixture );
			Customize( fixture, registry );
		}

		protected abstract void Customize( IFixture fixture, IServiceRegistry registry );
	}
}
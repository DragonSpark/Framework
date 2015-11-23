using DragonSpark.Activation;
using Ploeh.AutoFixture;
using System;

namespace DragonSpark.Testing.Framework
{
	[AttributeUsage( AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = true )]
	public abstract class RegistrationAttribute : Attribute, ICustomization
	{
		protected RegistrationAttribute( Type registrationType ) : this( registrationType, registrationType )
		{}

		protected RegistrationAttribute( Type registrationType, Type mappedTo )
		{
			RegistrationType = registrationType;
			MappedTo = mappedTo;
		}

		protected Type RegistrationType { get; }

		protected Type MappedTo { get; }

		void ICustomization.Customize( IFixture fixture )
		{
			var registry = DetermineRegistry( fixture );
			Customize( fixture, registry );
		}

		protected virtual IServiceRegistry DetermineRegistry( IFixture fixture )
		{
			var result = fixture.Create<IServiceRegistry>();
			return result;
		}

		protected abstract void Customize( IFixture fixture, IServiceRegistry registry );
	}
}
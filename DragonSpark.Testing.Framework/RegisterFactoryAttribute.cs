using DragonSpark.Activation;
using Ploeh.AutoFixture;
using System;

namespace DragonSpark.Testing.Framework
{
	public abstract class RegisterFactoryAttribute : RegistrationAttribute
	{
		protected RegisterFactoryAttribute( Type registrationType ) : base( registrationType )
		{}

		protected override void Customize( IFixture fixture, IServiceRegistry registry )
		{
			var factory = GetFactory( fixture, registry );
			registry.RegisterFactory( MappedTo, factory );
		}

		protected abstract Func<object> GetFactory( IFixture fixture, IServiceRegistry registry );
	}
}
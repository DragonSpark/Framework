using DragonSpark.Setup;
using Microsoft.Practices.Unity;
using System;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Setup.Registration;

namespace DragonSpark.Testing.TestObjects
{
	[Register]
	class RegisterAsSingleton
	{}

	[Register, LifetimeManager( typeof(TransientLifetimeManager) )]
	class RegisterAsMany
	{}

	class Class : IInterface
	{}
	class AnotherClass : IInterface
	{}

	public class YetAnotherClass : IInterface
	{}

	public class FactoryOfYAC : ActivateFactory<YetAnotherClass>
	{}

	class ClassWithBrokenConstructor : IInterface
	{
		public ClassWithBrokenConstructor()
		{
			throw new InvalidOperationException( "This is a broken constructor" );
		}
	}
}
using DragonSpark.Setup.Registration;
using Microsoft.Practices.Unity;

namespace DragonSpark.Testing.Objects
{
	[Register.Type, LifetimeManager( typeof(TransientLifetimeManager) )]
	public class RegisterAsMany
	{}
}
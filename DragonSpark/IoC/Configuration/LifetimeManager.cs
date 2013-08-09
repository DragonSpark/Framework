using DragonSpark.Configuration;

namespace DragonSpark.IoC.Configuration
{
	public abstract class LifetimeManager : Singleton<Microsoft.Practices.Unity.LifetimeManager>
	{}
}
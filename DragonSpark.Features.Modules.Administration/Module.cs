using DragonSpark.Application;
using Microsoft.Practices.Unity;

namespace DragonSpark.Features.Modules.Administration
{
	public class Module : ApplicationModule<Configuration>
	{
		public Module( IUnityContainer container, IModuleMonitor monitor ) : base( container, monitor )
		{}
	}
}

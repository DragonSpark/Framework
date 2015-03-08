using DragonSpark.Application;
using Microsoft.Practices.Unity;

namespace DragonSpark.Features.Modules.Authentication
{
	public class Module : ApplicationModule<Configuration>
	{
		public Module( IUnityContainer container, IModuleMonitor monitor ) : base( container, monitor )
		{}
	}

	/*public class AssignAuthenticationServiceOperation : MonitoredCommandBase<AuthenticationService>
	{
		readonly WebContextBase webContext;

		public AssignAuthenticationServiceOperation( WebContextBase webContext )
		{
			this.webContext = webContext;
		}

		protected override void ExecuteCommand( ICommandMonitor monitor )
		{
			webContext.Authentication = Context;
		}
	}*/
}

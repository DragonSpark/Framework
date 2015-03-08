using DragonSpark.Extensions;
using DragonSpark.IoC;
using Microsoft.Practices.ServiceLocation;
using System.Web;

namespace DragonSpark.Application.Communication
{
    public class LauncherModule<TLauncher> : IServiceLocatorModule where TLauncher : Launcher, new()
    {
        HttpApplication Context { get; set; }
		
        public void Init( HttpApplication context )
        {
			Context = context;

            lock ( context.Application )
            {
	            ServiceLocator = ServiceLocator ?? CreateServiceLocator();
	        }
        }

        static IServiceLocator CreateServiceLocator()
        {
            var launcher = new TLauncher();
            launcher.Run();
            var result = launcher.ServiceLocator;
            return result;
        }

    	void IHttpModule.Dispose()
        {}

        public IServiceLocator ServiceLocator
        {
            get { return Context.Application.Get( GetType().AssemblyQualifiedName ).To<IServiceLocator>(); }
            private set { Context.Application[ GetType().AssemblyQualifiedName ] = value; }
        }
    }
}
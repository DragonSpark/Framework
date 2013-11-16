using System;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using DragonSpark.Server;
using System.Web.Http;
using Microsoft.Cci;
using Microsoft.Samples.Debugging.CorSymbolStore;

namespace DragonSpark.Application.Server
{
	public class MvcApplication : HttpApplication
	{
		protected void Application_Start()
		{
			var initializer = new ApplicationInitializer( new Configuration.Server().Instance );
			initializer.Initialize( this, GlobalConfiguration.Configuration );
			// BundleTable.EnableOptimizations = true;
		}
	}
}
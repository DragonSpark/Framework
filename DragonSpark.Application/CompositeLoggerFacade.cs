using DragonSpark.Activation;
using DragonSpark.Extensions;
using Prism.Logging;
using Prism.Modularity;
using System.Collections.Generic;
using System.Reflection;
using AssemblyProvider = DragonSpark.Application.Runtime.AssemblyProvider;

namespace DragonSpark.Application
{
	public class CompositeLoggerFacade : ILoggerFacade
	{
		readonly IEnumerable<ILoggerFacade> loggers;

		public CompositeLoggerFacade( params ILoggerFacade[] loggers )
		{
			this.loggers = loggers;
		}

		public void Log( string message, Category category, Prism.Logging.Priority priority )
		{
			loggers.Apply( logger => logger.Log( message, category, priority ) );
		}
	}

	public class AssemblyModuleCatalog : Prism.Modularity.AssemblyModuleCatalog
	{
		readonly IAssemblyProvider assemblyProvider;

		public AssemblyModuleCatalog() : this( new AssemblyProvider(), new ModuleInfoBuilder() )
		{}

		public AssemblyModuleCatalog( IAssemblyProvider assemblyProvider, IModuleInfoBuilder builder ) : base( builder )
		{
			this.assemblyProvider = assemblyProvider;
		}

		protected override IEnumerable<Assembly> GetAssemblies()
		{
			return assemblyProvider.GetAssemblies();
		}
	}
}
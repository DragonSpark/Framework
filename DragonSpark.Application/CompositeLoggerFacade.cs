using DragonSpark.Extensions;
using Prism.Logging;
using Prism.Modularity;
using System.Collections.Generic;
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
		public AssemblyModuleCatalog() : this( AssemblyProvider.Instance, new DynamicModuleInfoBuilder() )
		{}

		public AssemblyModuleCatalog( IAssemblyProvider provider, IModuleInfoBuilder builder ) : base( provider, builder )
		{}
	}
}
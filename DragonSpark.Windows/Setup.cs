using DragonSpark.Logging;
using DragonSpark.Setup;

namespace DragonSpark.Windows
{
	public class Setup<TLoggingFacade> : Setup<TLoggingFacade, AssemblyModuleCatalog> where TLoggingFacade : ILoggerFacade, new()
	{}
}
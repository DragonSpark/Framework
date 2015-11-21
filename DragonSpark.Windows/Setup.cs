using DragonSpark.Diagnostics;
using DragonSpark.Setup;

namespace DragonSpark.Windows
{
	public class Setup<TLoggingFacade> : Setup<TLoggingFacade, AssemblyModuleCatalog> where TLoggingFacade : ILogger, new()
	{}
}
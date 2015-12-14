using DragonSpark.Diagnostics;
using DragonSpark.Setup;

namespace DragonSpark.Windows
{
	public class Setup<TLogging> : DragonSpark.Setup.Setup<CommandCollection<TLogging, AssemblyModuleCatalog>> where TLogging : ILogger, new()
	{}
}
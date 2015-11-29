using DragonSpark.Diagnostics;
using DragonSpark.Setup;

namespace DragonSpark.Windows
{
	public class Setup<TLogging> : Setup<TLogging, AssemblyModuleCatalog> where TLogging : ILogger, new()
	{}
}
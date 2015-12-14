using DragonSpark.Diagnostics;
using DragonSpark.Modularity;
using DragonSpark.Setup.Commands;
using DragonSpark.Setup.Registration;
using System.Windows.Input;

namespace DragonSpark.Setup
{
	public class CommandCollection<TLogger, TModuleCatalog> : CommandCollection
		where TLogger : ILogger, new()
		where TModuleCatalog : IModuleCatalog, new()
	{
		public CommandCollection() : base( new ICommand[]
		{
			new SetupLoggingCommand<TLogger>(),
			new SetupModuleCatalogCommand<TModuleCatalog>(),
			new RegisterFrameworkExceptionTypesCommand()
		} )
		{}
	}
}
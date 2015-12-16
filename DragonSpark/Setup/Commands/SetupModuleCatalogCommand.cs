using DragonSpark.Modularity;
using DragonSpark.Properties;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Setup.Commands
{
	public class SetupModuleCatalogCommand : SetupCommand
	{
		[Required( /*ErrorMessage = Resources.NullModuleCatalogException*/ )]
		public IModuleCatalog ModuleCatalog { get; set; }
		
		protected override void Execute( SetupContext context )
		{
			context.MessageLogger.Information( Resources.CreatingModuleCatalog, Priority.Low );
			
			context.Register( ModuleCatalog );

			// context.MessageLogger.Information( Resources.ConfiguringModuleCatalog, Priority.Low );
		}
	}

	/*public class SetupModuleCatalogCommand<TModuleCatalog> : SetupModuleCatalogCommandBase where TModuleCatalog : IModuleCatalog, new()
	{
		protected override IModuleCatalog CreateModuleCatalog()
		{
			var result = new TModuleCatalog();
			return result;
		}
	}*/
}
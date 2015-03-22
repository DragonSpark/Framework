using Prism.Logging;
using Prism.Modularity;

namespace DragonSpark.Testing.Client.Modularity
{
	[Module( ModuleName = "A Module! WOOOO!!!" )]
	public class Class1 : IModule
	{
		readonly ILoggerFacade logger;

		public Class1( ILoggerFacade logger )
		{
			this.logger = logger;
		}

		public void Initialize()
		{
			logger.Log( "SUCCESS!", Category.Info, Priority.High );
		}
	}
}

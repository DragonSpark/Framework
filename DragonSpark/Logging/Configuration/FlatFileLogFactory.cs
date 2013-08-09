using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Formatters;
using Microsoft.Practices.Unity;
using System.Diagnostics.Tracing;

namespace DragonSpark.Logging.Configuration
{
	public class FlatFileLogFactory : FileLogFactoryBase
	{
		protected override EventListener CreateListener( IUnityContainer container, IEventTextFormatter formatter )
		{
			var result = FlatFileLog.CreateListener( LogFilePath, formatter, IsAsynchronus );
			return result;
		}
	}
}
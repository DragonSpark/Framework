using DragonSpark.Extensions;
using DragonSpark.IoC.Configuration;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Formatters;
using Microsoft.Practices.Unity;
using System.Diagnostics.Tracing;

namespace DragonSpark.Logging.Configuration
{
	public class ConsoleLogFactory : EventListenerFactory
	{
		public NamedTypeBuildKey ColorMapper { get; set; }

		protected override EventListener CreateListener( IUnityContainer container, IEventTextFormatter formatter )
		{
			var result = ConsoleLog.CreateListener( formatter, ColorMapper.Transform( x => x.Create<IConsoleColorMapper>( container ) ) );
			return result;
		}
	}
}
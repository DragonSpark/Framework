using System.Linq;
using DragonSpark.Extensions;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace DragonSpark.Application.Logging
{
	public class TraceListenerLocator
	{
		readonly LogWriter writer;

		public TraceListenerLocator( LogWriter writer )
		{
			this.writer = writer;
		}

		public TTraceListener Locate<TTraceListener>( string name = null )
		{
			var query = 
				from source in writer.TraceSources.Values
				from wrapper in source.Listeners.OfType<ReconfigurableTraceListenerWrapper>()
				let traceListener = wrapper.InnerTraceListener
				where traceListener is TTraceListener && ( name == null || wrapper.Name == name )
				select traceListener.To<TTraceListener>();
			var result = query.Distinct().FirstOrDefault();
			return result;
		}

	}
}
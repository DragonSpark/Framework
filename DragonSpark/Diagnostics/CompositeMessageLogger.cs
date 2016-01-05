using DragonSpark.Extensions;
using System;
using System.Linq;

namespace DragonSpark.Diagnostics
{
	public class CompositeMessageLogger : MessageLoggerBase, IDisposable
	{
		readonly IMessageLogger[] loggers;

		public CompositeMessageLogger( params IMessageLogger[] loggers )
		{
			this.loggers = loggers;
		}

		protected override void OnLog( Message message ) => loggers.Each( logger => logger.Log( message ) );

		protected virtual void Dispose( bool disposing )
		{
			if ( disposing )
			{
				loggers.OfType<IDisposable>().Each( facade => facade.Dispose() );
			}
		}

		///<summary>
		///Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		///</summary>
		/// <remarks>Calls <see cref="Dispose(bool)"/></remarks>.
		///<filterpriority>2</filterpriority>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
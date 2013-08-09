using DragonSpark.Objects;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Formatters;
using Microsoft.Practices.Unity;
using System;
using System.Diagnostics.Tracing;

namespace DragonSpark.Logging.Configuration
{
	public class SqlDatabaseLogFactory : EventListenerFactory
	{
		public string InstanceName { get; set; }

		public string ConnectionString { get; set; }

		[DefaultPropertyValue( "Traces" )]
		public string TableName { get; set; }

		public TimeSpan? BufferingInterval { get; set; }

		[DefaultPropertyValue( 1000 )]
		public int BufferingCount { get; set; }

		public TimeSpan? ListenerDisposeTimeout { get; set; }

		[DefaultPropertyValue( 30000 )]
		public int MaxBufferSize { get; set; }

		protected override EventListener CreateListener( IUnityContainer container, IEventTextFormatter formatter )
		{
			var result = SqlDatabaseLog.CreateListener( InstanceName, ConnectionString, TableName, BufferingInterval, BufferingCount, ListenerDisposeTimeout, MaxBufferSize );
			return result;
		}
	}
}
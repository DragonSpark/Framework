using System.Diagnostics.Tracing;
using DragonSpark.Objects;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Sinks;
using Microsoft.Practices.Unity;

namespace DragonSpark.Logging.Configuration
{
	public class RollingFlatFileLogFactory : FileLogFactoryBase
	{
		[DefaultPropertyValue( 0 )]
		public int MaximumArchivedFilesAllowed { get; set; }

		[DefaultPropertyValue( 10 )]
		public int MaximumFileSizeInKb { get; set; }

		[DefaultPropertyValue( RollInterval.Day )]
		public RollInterval Interval { get; set; }

		[DefaultPropertyValue( "yyyy" )]
		public string TimestampPattern { get; set; }

		[DefaultPropertyValue( RollFileExistsBehavior.Increment )]
		public RollFileExistsBehavior RollFileExistsBehavior { get; set; }

		protected override EventListener CreateListener( IUnityContainer container, IEventTextFormatter formatter )
		{
			var result = RollingFlatFileLog.CreateListener( LogFilePath, MaximumFileSizeInKb, TimestampPattern, RollFileExistsBehavior, Interval, formatter, MaximumArchivedFilesAllowed, IsAsynchronus );
			return result;
		}
	}
}
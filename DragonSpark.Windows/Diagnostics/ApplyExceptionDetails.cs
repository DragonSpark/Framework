using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using Serilog;
using Serilog.Exceptions;
using Serilog.Exceptions.Destructurers;
using System.Composition;

namespace DragonSpark.Windows.Diagnostics
{
	public sealed class ApplyExceptionDetails : AlterationBase<LoggerConfiguration>
	{
		[Export( typeof(IAlteration<LoggerConfiguration>) )]
		public static ApplyExceptionDetails Default { get; } = new ApplyExceptionDetails();
		ApplyExceptionDetails() {}

		public override LoggerConfiguration Get( LoggerConfiguration parameter ) => 
			parameter.Enrich.WithExceptionDetails( new SuppliedAndExportedItems<IExceptionDestructurer>( ExceptionEnricher.DefaultDestructurers ) );
	}
}
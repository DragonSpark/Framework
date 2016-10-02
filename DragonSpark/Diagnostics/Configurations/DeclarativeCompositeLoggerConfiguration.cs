using DragonSpark.Commands;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Sources.Parameterized;
using Serilog;
using System.Linq;
using System.Windows.Markup;

namespace DragonSpark.Diagnostics.Configurations
{
	[ContentProperty( nameof(Commands) )]
	public class DeclarativeCompositeLoggerConfiguration : AlterationBase<LoggerConfiguration>
	{
		public DeclarativeCollection<CommandBase<LoggerConfiguration>> Commands { get; } = new DeclarativeCollection<CommandBase<LoggerConfiguration>>();

		public override LoggerConfiguration Get( LoggerConfiguration parameter ) => Commands.Aggregate( parameter, ( loggerConfiguration, command ) => loggerConfiguration.With( command.Execute ) );
	}
}
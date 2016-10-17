using DragonSpark.Commands;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Sources.Parameterized;
using System.Linq;
using System.Windows.Markup;

namespace DragonSpark.Diagnostics.Configurations
{
	[ContentProperty( nameof(Commands) )]
	public class LoggerConfiguration : AlterationBase<Serilog.LoggerConfiguration>
	{
		public DeclarativeCollection<CommandBase<Serilog.LoggerConfiguration>> Commands { get; } = new DeclarativeCollection<CommandBase<Serilog.LoggerConfiguration>>();

		public override Serilog.LoggerConfiguration Get( Serilog.LoggerConfiguration parameter ) => Commands.Aggregate( parameter, ( loggerConfiguration, command ) => loggerConfiguration.With( command.Execute ) );
	}
}
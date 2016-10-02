using DragonSpark.Commands;
using DragonSpark.Diagnostics.Configurations;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using Serilog;
using Serilog.Core;
using System;

namespace DragonSpark.Diagnostics
{
	sealed class DefaultLoggerAlterations : ItemSource<IAlteration<LoggerConfiguration>>
	{
		readonly static IAlteration<LoggerConfiguration> LogContext = EnrichFromLogContextCommand.Default.ToAlteration();

		public static DefaultLoggerAlterations Default { get; } = new DefaultLoggerAlterations();
		DefaultLoggerAlterations() : base( LogContext, FormatterConfiguration.Default, ControllerAlteration.DefaultNested, ApplicationAssemblyAlteration.Default ) {}

		sealed class ControllerAlteration : AlterationBase<LoggerConfiguration>
		{
			public static ControllerAlteration DefaultNested { get; } = new ControllerAlteration();
			ControllerAlteration() : this( LoggingController.Default.Get ) {}

			readonly Func<LoggingLevelSwitch> controller;

			ControllerAlteration( Func<LoggingLevelSwitch> controller )
			{
				this.controller = controller;
			}

			public override LoggerConfiguration Get( LoggerConfiguration parameter ) => parameter.MinimumLevel.ControlledBy( controller() );
		}
	}
	
}
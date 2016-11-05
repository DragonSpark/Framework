using DragonSpark.Aspects.Specifications;
using DragonSpark.Aspects.Validation;
using DragonSpark.Commands;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Scopes;
using DragonSpark.Specifications;
using Serilog;
using System;

namespace DragonSpark.Diagnostics
{
	public sealed class Logger : ConfiguringFactory<object, ILogger>
	{
		readonly static Func<object, ILogger> Factory = LoggerFactory.Default.ToDelegate();
		readonly static Action<ILogger> Configure = Command.Implementation.ToDelegate();

		public static IParameterizedSource<object, ILogger> Default { get; } = new ParameterizedSingletonScope<object, ILogger>( o => new Logger().Get( o ) );
		Logger() : base( Factory, Configure ) {}
		
		[ApplyAutoValidation, ApplySpecification( typeof(OncePerScopeSpecification<ILogger>) )]
		sealed class Command : CommandBase<ILogger>
		{
			public static Command Implementation { get; } = new Command();
			Command() {}

			public override void Execute( ILogger parameter )
			{
				SystemLogger.Default.Assign( Defaults.Factory );
				PurgeLoggerHistoryCommand.Default.Execute( parameter.Write );
			}
		}
	}
}
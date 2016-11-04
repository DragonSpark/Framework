using DragonSpark.Sources;
using DragonSpark.Sources.Scopes;
using Serilog;
using System;

namespace DragonSpark.Diagnostics
{
	public static class Defaults
	{
		public const string Template = "{Timestamp:HH:mm:ss:fff} [{Level}] ({SourceContext}) {Message}{NewLine}{Exception}";

		public static ISource<ILogger> Source { get; } = Logger.Default.ToExecutionSingletonScope();

		public static Func<ILogger> Factory { get; } = Source.Get;
	}
}
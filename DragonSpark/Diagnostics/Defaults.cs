using DragonSpark.Sources;
using Serilog;
using System;
using DragonSpark.Sources.Scopes;

namespace DragonSpark.Diagnostics
{
	public static class Defaults
	{
		public const string Template = "{Timestamp:HH:mm:ss:fff} [{Level}] ({SourceContext}) {Message}{NewLine}{Exception}";

		public static ISource<ILogger> Source { get; } = Logger.Default.ToExecutionScope();

		public static Func<ILogger> Factory { get; } = Source.Get;
	}
}
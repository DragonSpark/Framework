using DragonSpark.Application;
using System;

namespace DragonSpark.Sources
{
	public static class Defaults
	{
		public static string TimerTemplate { get; } = "Executing Delegate {@Method}.";
		public static string ParameterizedTimerTemplate { get; } = "Executing Delegate {@Method} with parameter {Parameter}.";
		public static Func<IExportProvider> Exports { get; } = Application.Exports.Default.Get;

		public static Func<Type, Func<object, object>> AccountedSource { get; } = SourceAccountedAlteration.Defaults.Get;
	}
}
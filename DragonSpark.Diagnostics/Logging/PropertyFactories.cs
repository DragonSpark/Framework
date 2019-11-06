using Serilog.Core;
using Serilog.Events;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Diagnostics.Logging
{
	sealed class PropertyFactories : ReferenceValueTable<LogEvent, ILogEventPropertyFactory>
	{
		public static PropertyFactories Default { get; } = new PropertyFactories();

		PropertyFactories() {}
	}
}
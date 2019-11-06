using Serilog;
using Serilog.Configuration;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using System;

namespace DragonSpark.Diagnostics.Logging.Configuration
{
	sealed class ScalarConfiguration<T> : Select<LoggerDestructuringConfiguration, LoggerConfiguration>,
	                                      ILoggingDestructureConfiguration
	{
		public static ScalarConfiguration<T> Default { get; } = new ScalarConfiguration<T>();

		ScalarConfiguration() : base(x => x.AsScalar<T>()) {}
	}

	sealed class ScalarConfiguration : ILoggingDestructureConfiguration
	{
		readonly Array<Type> _types;

		public ScalarConfiguration(params Type[] types) : this(types.Result()) {}

		public ScalarConfiguration(Array<Type> types) => _types = types;

		public LoggerConfiguration Get(LoggerDestructuringConfiguration parameter) => _types.Reference()
		                                                                                    .Alter(parameter.AsScalar);
	}
}
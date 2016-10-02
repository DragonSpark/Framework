using DragonSpark.Configuration;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Specifications;
using DragonSpark.TypeSystem;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Immutable;

namespace DragonSpark.Diagnostics
{
	public abstract class LoggerBase : ConfigurableParameterizedFactoryBase<LoggerConfiguration, ILogger>
	{
		protected LoggerBase() : this( Items<IAlteration<LoggerConfiguration>>.Default ) {}
		protected LoggerBase( params IAlteration<LoggerConfiguration>[] configurations ) : this( configurations.ToImmutableArray() ) {}
		protected LoggerBase( ImmutableArray<IAlteration<LoggerConfiguration>> configurations ) : base( o => new LoggerConfiguration(), configurations.Wrap(), ( configuration, parameter ) => configuration.CreateLogger().ForContext( Constants.SourceContextPropertyName, parameter, Specification.Default.IsSatisfiedBy( parameter ) ) ) {}

		sealed class Specification : DelegatedAssignedSpecification<object, IFormattable>
		{
			public static Specification Default { get; } = new Specification();
			Specification() : base( FormattableSource.Default.Get ) {}
		}
	}
}
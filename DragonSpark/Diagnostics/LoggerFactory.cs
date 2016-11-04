using DragonSpark.Activation;
using DragonSpark.Sources.Coercion;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Scopes;
using DragonSpark.Specifications;
using JetBrains.Annotations;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Immutable;

namespace DragonSpark.Diagnostics
{
	public class LoggerFactory : ParameterizedSourceBase<object, ILogger>
	{
		public static LoggerFactory Default { get; } = new LoggerFactory();
		LoggerFactory() : this ( LoggerAlterations.Default.Get() ) {}

		public LoggerFactory( params IAlteration<Serilog.LoggerConfiguration>[] alterations ) : this( alterations.ToImmutableArray() ) {}
		public LoggerFactory( ImmutableArray<IAlteration<Serilog.LoggerConfiguration>> alterations ) : this( new LoggerConfigurationFactory( alterations ).ToScope() ) {}

		[UsedImplicitly]
		public LoggerFactory( IParameterizedScope<object, Serilog.LoggerConfiguration> configuration )
		{
			Configuration = configuration;
		}

		public IParameterizedScope<object, Serilog.LoggerConfiguration> Configuration { get; }

		public override ILogger Get( object parameter ) =>
			Configuration
				.CoerceTo<Serilog.LoggerConfiguration, LoggerConfiguration>()
				.CoerceTo( LoggerCreator.Implementation )
				.Get( parameter );

		[UsedImplicitly]
		public sealed class LoggerConfigurationFactory : AggregateParameterizedSource<Serilog.LoggerConfiguration>
		{
			public LoggerConfigurationFactory( ImmutableArray<IAlteration<Serilog.LoggerConfiguration>> alterations ) : base( ParameterConstructor<LoggerConfiguration>.Default, alterations ) {}
		}

		public sealed class LoggerCreator : ParameterizedSourceBase<LoggerConfiguration, ILogger>
		{
			public static IParameterizedScope<Serilog.LoggerConfiguration, ILogger> Implementation { get; } = new LoggerCreator().Coerce( CastCoercer<Serilog.LoggerConfiguration, LoggerConfiguration>.Default ).ToScope();
			LoggerCreator() {}

			public override ILogger Get( LoggerConfiguration parameter ) => 
				parameter.CreateLogger().ForContext( Constants.SourceContextPropertyName, parameter.Instance, Specification.Instance.IsSatisfiedBy( parameter ) );

			sealed class Specification : DelegatedAssignedSpecification<object, IFormattable>
			{
				public static Specification Instance { get; } = new Specification();
				Specification() : base( FormattableSource.Default.Get ) {}
			}
		}

		[UsedImplicitly]
		public sealed class LoggerConfiguration : Serilog.LoggerConfiguration
		{
			public LoggerConfiguration( object instance )
			{
				Instance = instance;
			}

			public object Instance { get; }
		}
	}
}
using DragonSpark.Activation;
using DragonSpark.Sources;
using DragonSpark.Sources.Coercion;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Scopes;
using DragonSpark.Specifications;
using JetBrains.Annotations;
using Serilog;
using Serilog.Core;
using System;

namespace DragonSpark.Diagnostics
{
	public class LoggerFactory : ParameterizedSourceBase<object, ILogger>
	{
		public static LoggerFactory Default { get; } = new LoggerFactory();
		LoggerFactory() : this ( LoggerAlterations.Default ) {}

		/*public LoggerFactory( params IAlteration<Serilog.LoggerConfiguration>[] alterations ) : this( alterations.ToImmutableArray() ) {}*/
		public LoggerFactory( IItemSource<IAlteration<Serilog.LoggerConfiguration>> alterations ) : this( new LoggerConfigurationCreator( alterations ).ToScope() ) {}

		[UsedImplicitly]
		public LoggerFactory( IParameterizedScope<object, Serilog.LoggerConfiguration> configuration )
		{
			Configuration = configuration;
		}

		public IParameterizedScope<object, Serilog.LoggerConfiguration> Configuration { get; }

		public override ILogger Get( object parameter ) =>
			Configuration
				.To<Serilog.LoggerConfiguration, LoggerConfiguration>()
				.To( Factory.Implementation )
				.Get( parameter );

		[UsedImplicitly]
		sealed class LoggerConfigurationCreator : AggregateParameterizedSource<Serilog.LoggerConfiguration>
		{
			public LoggerConfigurationCreator( IItemSource<IAlteration<Serilog.LoggerConfiguration>> alterations ) : base( ParameterConstructor<LoggerConfiguration>.Default, alterations ) {}
		}

		[UsedImplicitly]
		public sealed class Factory : ParameterizedSourceBase<LoggerConfiguration, ILogger>
		{
			public static IParameterizedScope<Serilog.LoggerConfiguration, ILogger> Implementation { get; } = new Factory().Accept( CastCoercer<Serilog.LoggerConfiguration, LoggerConfiguration>.Default ).ToScope();
			Factory() {}

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
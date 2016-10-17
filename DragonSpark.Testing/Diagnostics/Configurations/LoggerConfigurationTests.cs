using DragonSpark.Diagnostics;
using DragonSpark.Diagnostics.Configurations;
using DragonSpark.Extensions;
using DragonSpark.Specifications;
using JetBrains.Annotations;
using Moq;
using Serilog.Configuration;
using Serilog.Context;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Xunit;

namespace DragonSpark.Testing.Diagnostics.Configurations
{
	public class LoggerConfigurationTests
	{
		[Fact]
		public void MinimumLevelIs()
		{
			var configuration = new LoggerConfiguration
								{
									Commands =
									{
										new MinimumLevelIsCommand { Level = LogEventLevel.Verbose }
									}
								};
			var configured = configuration.Get( new Serilog.LoggerConfiguration() );
			var logger = configured.CreateLogger();
			Assert.True( logger.IsEnabled( LogEventLevel.Verbose ) );
		}

		[Fact]
		public void MinimumLevelSwitch()
		{
			var controller = new LoggingLevelSwitch();
			var configuration = new LoggerConfiguration
								{
									Commands =
									{
										new MinimumLevelSwitchCommand { Controller = controller }
									}
								};
			var configured = configuration.Get( new Serilog.LoggerConfiguration() );
			var logger = configured.CreateLogger();
			Assert.False( logger.IsEnabled( LogEventLevel.Verbose ) );
			controller.MinimumLevel = LogEventLevel.Verbose;
			Assert.True( logger.IsEnabled( LogEventLevel.Verbose ) );
		}

		[Fact]
		public void ReadFromKeyValuePairs()
		{
			var dictionary = new Dictionary<string, string> { { "minimum-level", LogEventLevel.Verbose.ToString() } };
			var configuration = new LoggerConfiguration
								{
									Commands =
									{
										new ReadFromKeyValuePairsCommand { Dictionary =  dictionary }
									}
								};
			var configured = configuration.Get( new Serilog.LoggerConfiguration() );
			var logger = configured.CreateLogger();
			Assert.True( logger.IsEnabled( LogEventLevel.Verbose ) );
		}

		[Fact]
		public void ReadFromSettings()
		{
			var configuration = new LoggerConfiguration
								{
									Commands =
									{
										new ReadFromSettingsCommand { Settings = Settings.Default }
									}
								};
			var configured = configuration.Get( new Serilog.LoggerConfiguration() );
			var logger = configured.CreateLogger();
			Assert.True( logger.IsEnabled( LogEventLevel.Verbose ) );
		}

		[Theory, Framework.Application.AutoData]
		public void Filter( Mock<ILogEventFilter> filter )
		{
			var configuration = new LoggerConfiguration
								{
									Commands =
									{
										new FilterCommand { Items = { filter.Object } }
									}
								};
			var configured = configuration.Get( new Serilog.LoggerConfiguration() );
			var logger = configured.CreateLogger();
			logger.Information( "Hello World!" );
			filter.Verify( eventFilter => eventFilter.IsEnabled( It.IsAny<LogEvent>() ), Times.Once() );
		}

		[Theory, Framework.Application.AutoData]
		public void FilterByExcluding( Mock<ISpecification<LogEvent>> specification )
		{
			var configuration = new LoggerConfiguration
								{
									Commands =
									{
										new FilterByExcludingCommand { Specification = specification.Object }
									}
								};
			var configured = configuration.Get( new Serilog.LoggerConfiguration() );
			var logger = configured.CreateLogger();
			logger.Information( "Hello World!" );
			specification.Verify( eventFilter => eventFilter.IsSatisfiedBy( It.IsAny<LogEvent>() ), Times.Once() );
		}

		[Theory, Framework.Application.AutoData]
		public void FilterByIncludingOnly( Mock<ISpecification<LogEvent>> specification )
		{
			var configuration = new LoggerConfiguration
								{
									Commands =
									{
										new FilterByIncludingOnlyCommand { Specification = specification.Object }
									}
								};
			var configured = configuration.Get( new Serilog.LoggerConfiguration() );
			var logger = configured.CreateLogger();
			logger.Information( "Hello World!" );
			specification.Verify( eventFilter => eventFilter.IsSatisfiedBy( It.IsAny<LogEvent>() ), Times.Once() );
		}

		[Fact]
		public void EnrichFromLogContext()
		{
			var configuration = new LoggerConfiguration
								{
									Commands =
									{
										EnrichFromLogContextCommand.Default
									}
								};
			var configured = configuration.Get( new Serilog.LoggerConfiguration() );
			var history = new LoggerHistorySink();
			var logger = configured.WriteTo.Sink( history ).CreateLogger();
			using ( LogContext.PushProperty( "Testing", 123 ) )
			{
				logger.Information( "Hello World!" );
			}
			var item = history.Events.Only();
			Assert.True( item.Properties.ContainsKey( "Testing" ) );
		}

		[Fact]
		public void EnrichWithProperty()
		{
			var configuration = new LoggerConfiguration
								{
									Commands =
									{
										new EnrichWithPropertyCommand { PropertyName = "EnrichWithProperty", Value = 123, DestructureObjects = false }
									}
								};
			var configured = configuration.Get( new Serilog.LoggerConfiguration() );
			var history = new LoggerHistorySink();
			var logger = configured.WriteTo.Sink( history ).CreateLogger();
			logger.Information( "Hello World!" );
			var item = history.Events.Only();
			Assert.True( item.Properties.ContainsKey( "EnrichWithProperty" ) );
		}

		[Theory, Framework.Application.AutoData]
		public void Enrich( Mock<ILogEventEnricher> enricher )
		{
			var configuration = new LoggerConfiguration
								{
									Commands =
									{
										new EnrichCommand { Items = { enricher.Object } }
									}
								};
			var configured = configuration.Get( new Serilog.LoggerConfiguration() );
			var logger = configured.CreateLogger();
			logger.Information( "Hello World!" );
			enricher.Verify( item => item.Enrich( It.IsAny<LogEvent>(), It.IsAny<ILogEventPropertyFactory>() ), Times.Once() );
		}

		[Fact]
		public void DestructureType()
		{
			var configuration = new LoggerConfiguration
								{
									Commands =
									{
										new DestructureTypeCommand { ScalarType = typeof(Item) }
									}
								};
			var configured = configuration.Get( new Serilog.LoggerConfiguration() );
			var history = new LoggerHistorySink();
			var logger = configured.WriteTo.Sink( history ).CreateLogger();
			logger.Information( "Hello World! {@Item}", new Item { PropertyName = "Hello Hello!" } );

			var item = history.Events.Only();
			Assert.IsType<ScalarValue>( item.Properties.Only().Value );
		}

		[Fact]
		public void DestructureMaximumDepth()
		{
			var configuration = new LoggerConfiguration
								{
									Commands =
									{
										new DestructureMaximumDepthCommand { MaximumDepth = 67 }
									}
								};
			var configured = configuration.Get( new Serilog.LoggerConfiguration() );
			var depth = (int)configured.GetType().GetField( "_maximumDestructuringDepth", BindingFlags.NonPublic | BindingFlags.Instance ).GetValue( configured );
			Assert.Equal( 67, depth );
		}

		[Theory, Framework.Application.AutoData]
		public void Destructure( Mock<IDestructuringPolicy> policy )
		{
			var configuration = new LoggerConfiguration
								{
									Commands =
									{
										new DestructureCommand { Policies = { policy.Object } }
									}
								};
			var configured = configuration.Get( new Serilog.LoggerConfiguration() );
			var logger = configured.CreateLogger();

			LogEventPropertyValue result = null;
			policy.Setup( item => item.TryDestructure( It.IsAny<object>(), It.IsAny<ILogEventPropertyValueFactory>(), out result ) );
			logger.Information( "Hello World!" );
			Assert.Null( result );
		}

		[Fact]
		public void DestructureByTransform()
		{
			var count = 0;
			Func<Item, object> transform = o =>
										   {
											   count++;
											   return o.ToString();
										   };

			var configuration = new LoggerConfiguration
								{
									Commands =
									{
										new DestructureByTransformCommand<Item> { Transform = transform }
									}
								};
			var configured = configuration.Get( new Serilog.LoggerConfiguration() );
			var logger = configured.CreateLogger();

			logger.Information( "Hello World! {@Item}", new Item { PropertyName = "Hello Hello!" } );
			Assert.Equal( 1, count );
		}

		[Fact]
		public void AddTextWriter()
		{
			var writer = new StringWriter();
			var configuration = new LoggerConfiguration
								{
									Commands =
									{
										new AddTextWriterCommand { Writer = writer, FormatProvider = null }
									}
								};
			var configured = configuration.Get( new Serilog.LoggerConfiguration() );
			var logger = configured.CreateLogger();
			const string template = "Hello World!";
			logger.Information( template );
			Assert.Contains( template, writer.GetStringBuilder().ToString() );
		}

		[Fact]
		public void AddSeqCommand()
		{
			var configuration = new LoggerConfiguration
								{
									Commands =
									{
										new AddSeqSinkCommand { Endpoint = new Uri( "http://localhost/" ) }
									}
								};
			var configured = configuration.Get( new Serilog.LoggerConfiguration() );
			string message = null;
			SelfLog.Enable( s => message = s );

			using ( var logger = configured.CreateLogger() )
			{
				logger.Information( "Hello World!" );
			}
			Assert.Contains( "Exception while emitting periodic batch from Serilog.Sinks.Seq.SeqSink: System.AggregateException: One or more errors occurred.", message );
		}

		class Item
		{
			[UsedImplicitly]
			public string PropertyName { get; set; }
		}

		sealed class Settings : ILoggerSettings
		{
			public static Settings Default { get; } = new Settings();
			Settings() {}

			public void Configure( Serilog.LoggerConfiguration loggerConfiguration ) => loggerConfiguration.MinimumLevel.Verbose();
		}
	}
}
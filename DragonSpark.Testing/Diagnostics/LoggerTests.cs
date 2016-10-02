using DragonSpark.Activation.Location;
using DragonSpark.Application;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Application;
using DragonSpark.Testing.Framework.Application.Setup;
using DragonSpark.TypeSystem;
using Serilog;
using Serilog.Events;
using System;
using System.Composition;
using System.Linq;
using System.Reflection;
using Xunit;

namespace DragonSpark.Testing.Diagnostics
{
	[Trait( Traits.Category, Traits.Categories.ServiceLocation ), ContainingTypeAndNested, FormatterTypes]
	public class LoggerTests
	{
		[Theory, AutoData, FrameworkTypes]
		public void FormattingAsExpected( [Service]CompositionContext context, string text )
		{
			var logger = context.GetExport<ILogger>();

			var serviceProvider = DefaultServices.Default.Cached();
			Assert.Same( Logger.Default.Get( Execution.Current() ), serviceProvider.Get<ILogger>() );
			Assert.Same( serviceProvider.Get<ILogger>(), logger );

			var method = new Action( Subject ).Method;

			HelloWorld.Defaults.Get( logger ).Execute( text, method );
			
			var history = context.GetExport<ILoggerHistory>();
			Assert.Same( serviceProvider.Get<ILoggerHistory>(), history );
			var message = LogEventMessageFactory.Default.Get( history.Events ).Last();
			Assert.Contains( text, message );
			
			Assert.Contains( new MethodFormatter( method ).ToString(), message );
		}

		[Theory, AutoData]
		public void EnsureAssembly()
		{
			var logger = Logger.Default.Get( this );
			logger.Information( "Hello World!" );
			var line = LoggingHistory.Default.Get().Events.Single();
			var source = DefaultAssemblyInformationSource.Default.Get();
			var property = line.Properties[nameof(AssemblyInformation)].To<StructureValue>();
			Assert.NotNull( property );
			Assert.Equal( nameof(AssemblyInformation), property.TypeTag );
			Assert.Equal( typeof(AssemblyInformation).GetProperties().Length, property.Properties.Count );
			Assert.Equal( source.Title, property.Properties.Single( eventProperty => eventProperty.Name == "Title" ).Value.ToString().Trim( '"' ) );
		}

		static void Subject() {}

		sealed class HelloWorld : LogCommandBase<string, MethodBase>
		{
			public static IParameterizedSource<ILogger, HelloWorld> Defaults { get; } = new Cache<ILogger, HelloWorld>( logger => new HelloWorld( logger ) );
			HelloWorld( ILogger logger ) : base( logger, "Hello World! {Text} - {@Method}" ) {}
		}
	}
}
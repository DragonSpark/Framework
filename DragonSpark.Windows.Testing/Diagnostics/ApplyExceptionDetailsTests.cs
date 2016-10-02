using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Testing.Framework.Application;
using DragonSpark.Testing.Framework.Application.Setup;
using DragonSpark.Windows.Diagnostics;
using JetBrains.Annotations;
using Serilog.Events;
using Serilog.Exceptions.Destructurers;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using Xunit;

namespace DragonSpark.Windows.Testing.Diagnostics
{
	public class ApplyExceptionDetailsTests
	{
		[Theory, AutoData, AdditionalTypes( typeof(ApplyExceptionDetails) ), FrameworkTypes, FormatterTypes, ContainingTypeAndNested]
		public void DetailsWorkAsExpected( int number, string message )
		{
			var logger = Logger.Default.Get( this );
			logger.Information( new CustomException( number ), message );
			var line = LoggingHistory.Default.Get().Events.Single();
			Assert.True( line.Properties.ContainsKey( "ExceptionDetail" ) );
			var elements = line.Properties["ExceptionDetail"].To<DictionaryValue>().Elements;
			Assert.Equal( 6, elements.Count );
			Assert.Equal( number, elements[ elements.Keys.Single( value => (string)value.Value == "AwesomeNumber" ) ].To<ScalarValue>().Value );
		}

		class CustomException : Exception
		{
			public CustomException( int number )
			{
				Number = number;
			}

			public int Number { get; }
		}

		[Export( typeof(IExceptionDestructurer) ), UsedImplicitly]
		class Destructurer : ExceptionDestructurer
		{
			public override Type[] TargetTypes => typeof(CustomException).ToItem();

			public override void Destructure( Exception exception, IDictionary<string, object> data, Func<Exception, IDictionary<string, object>> innerDestructure )
			{
				base.Destructure( exception, data, innerDestructure );

				var custom = exception as CustomException;
				if ( custom != null )
				{
					data.Add( "AwesomeNumber", custom.Number );
				}
			}
		}
	}
}

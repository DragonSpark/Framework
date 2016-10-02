using DragonSpark.Activation;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Specifications;
using DragonSpark.TypeSystem;
using Serilog;
using System;

namespace DragonSpark.Diagnostics
{
	sealed class FormatterConfiguration : AlterationBase<LoggerConfiguration>
	{
		readonly static Func<object, object> Formatter = DragonSpark.Formatter.Default.Get;

		public static FormatterConfiguration Default { get; } = new FormatterConfiguration();
		FormatterConfiguration() {}

		public override LoggerConfiguration Get( LoggerConfiguration parameter )
		{
			foreach ( var type in KnownTypes.Default.Get<IFormattable>() )
			{
				var located = ConstructingParameterLocator.Default.Get( type );
				if ( located != null )
				{
					parameter.Destructure.ByTransformingWhere( TypeAssignableSpecification.Defaults.Get( located ).ToSpecificationDelegate(), Formatter );
				}
			}

			return parameter;
		}
	}
}
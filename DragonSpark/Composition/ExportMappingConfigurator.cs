using System;
using System.Collections.Immutable;
using System.Composition;
using System.Composition.Convention;
using System.Composition.Hosting;
using System.Linq;

namespace DragonSpark.Composition
{
	public sealed class ExportMappingConfigurator : ContainerConfigurator
	{
		[Export( typeof(ContainerConfigurator) )]
		public static ExportMappingConfigurator Default { get; } = new ExportMappingConfigurator();
		ExportMappingConfigurator() : this( ExportMappings.Default.Get ) {}

		readonly Func<ImmutableArray<ExportMapping>> source;

		public ExportMappingConfigurator( Func<ImmutableArray<ExportMapping>> source )
		{
			this.source = source;
		}

		public override ContainerConfiguration Get( ContainerConfiguration parameter )
		{
			var mappings = source();
			var builder = new ConventionBuilder();
			foreach ( var mapping in mappings )
			{
				builder.ForType( mapping.Subject ).Export( conventionBuilder => conventionBuilder.AsContractType( mapping.ExportAs ?? mapping.Subject ) );	
			}
			var subjects = mappings.Select( mapping => mapping.Subject );
			var result = parameter.WithParts( subjects, builder );
			return result;
		}
	}
}
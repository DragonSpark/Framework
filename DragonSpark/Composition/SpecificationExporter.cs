using DragonSpark.TypeSystem;
using System;
using System.Collections.Generic;
using System.Composition.Hosting.Core;

namespace DragonSpark.Composition
{
	// ReSharper disable once UnusedTypeParameter
	public sealed class SpecificationRequest<T> {}

	public sealed class SpecificationExporter : ExportDescriptorProviderBase
	{
		public static Type Definition { get; } = typeof(SpecificationRequest<>);

		public static SpecificationExporter Default { get; } = new SpecificationExporter();
		SpecificationExporter() : this( ExportsProfileFactory.Default.Get ) {}

		readonly Func<ExportsProfile> profileSource;

		SpecificationExporter( Func<ExportsProfile> profileSource )
		{
			this.profileSource = profileSource;
		}

		public override IEnumerable<ExportDescriptorPromise> GetExportDescriptors( CompositionContract contract, DependencyAccessor descriptorAccessor )
		{
			var adapter = contract.ContractType.Adapt();
			if ( adapter.IsGenericOf( Definition ) )
			{
				var inner = adapter.GetInnerType();
				var type = contract.ChangeType( inner );
				var exists = profileSource().IsSatisfiedBy( inner ) || Exists( descriptorAccessor, type );
				yield return new ExportDescriptorPromise( type, GetType().Name, true, NoDependencies, new Factory( exists ).Get );
			}
		}

		static bool Exists( DependencyAccessor descriptorAccessor, CompositionContract contract )
		{
			CompositionDependency dependency;
			var result = descriptorAccessor.TryResolveOptionalDependency( "Specification Exists Request", contract, true, out dependency );
			return result;
		}

		sealed class Factory : FactoryBase
		{
			readonly bool result;

			public Factory( bool result )
			{
				this.result = result;
			}

			protected override object Create( LifetimeContext context, CompositionOperation operation ) => result;
		}
	}
}
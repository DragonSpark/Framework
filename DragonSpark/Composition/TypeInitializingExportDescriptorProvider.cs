using DragonSpark.Commands;
using DragonSpark.Extensions;
using DragonSpark.TypeSystem;
using System;
using System.Collections.Generic;
using System.Composition.Hosting.Core;
using System.Linq;

namespace DragonSpark.Composition
{
	// https://github.com/dotnet/corefx/issues/6857
	public sealed class TypeInitializingExportDescriptorProvider : ExportDescriptorProvider
	{
		readonly static Action<Type> Initializer = InitializeTypeCommand.Default.ToDelegate();
		readonly static Func<Type, Type> Convention = ConventionImplementations.Default.Get;

		public static TypeInitializingExportDescriptorProvider Default { get; } = new TypeInitializingExportDescriptorProvider();
		TypeInitializingExportDescriptorProvider() : this( Convention ) {}

		readonly Func<Type, Type> convention;

		TypeInitializingExportDescriptorProvider( Func<Type, Type> convention )
		{
			this.convention = convention;
		}

		public override IEnumerable<ExportDescriptorPromise> GetExportDescriptors( CompositionContract contract, DependencyAccessor descriptorAccessor )
		{
			contract.ContractType
					.Append( convention( contract.ContractType ) )
					.WhereAssigned()
					.Distinct()
					.Each( Initializer );
			yield break;
		}
	}
}
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using DragonSpark.IoC;

namespace DragonSpark.Extensions
{
	public static class CompositionExtensions
	{
		public static TView Metadata<TView>( this ExportDefinition target )
		{
			var result = AttributedModelServices.GetMetadataView<TView>( target.Metadata );
			return result;
		}

		public static Type GetDecoratedType( this ExportDefinition target )
		{
			var result = Type.GetType( target.To<ICompositionElement>().Origin.DisplayName ).To<Type>();
			return result;
		}

		public static IEnumerable<ExportDefinition> ResolveDefinitions<TType>( this ExportProvider target, string source = null )
		{
			var definition = new ContractBasedImportDefinition( source ?? AttributedModelServices.GetContractName( typeof(TType) ), AttributedModelServices.GetContractName( typeof(TType) ), null, ImportCardinality.ZeroOrMore, false, false, CreationPolicy.Any );
			var result = target.GetExports( definition ).Select( x => x.Definition ).Distinct( ExportDefinitionComparer.Instance ).ToArray();
			return result;
		}
	}
}
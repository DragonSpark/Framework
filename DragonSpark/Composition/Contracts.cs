using DragonSpark.Sources.Parameterized.Caching;
using System.Collections.Immutable;
using System.Composition.Hosting.Core;

namespace DragonSpark.Composition
{
	public sealed class Contracts : CacheWithImplementedFactoryBase<CompositionContract, string>
	{
		public static Contracts Default { get; } = new Contracts();
		Contracts() {}

		protected override string Create( CompositionContract parameter )
		{
			object sharingBoundaryMetadata = null;
			var result = parameter.MetadataConstraints?.ToImmutableDictionary().TryGetValue( "SharingBoundary", out sharingBoundaryMetadata ) ?? false ? (string)sharingBoundaryMetadata : null;
			return result;
		}
	}
}
using System;
using System.Collections.Generic;

namespace DragonSpark.Composition
{
	public sealed class ConventionMappings : DescriptorCollectionBase<ConventionMapping>
	{
		public ConventionMappings( IEnumerable<ConventionMapping> items ) : base( items, mapping => mapping.ImplementationType ) {}

		protected override Type GetKeyForItem( ConventionMapping item ) => item.InterfaceType;
	}
}
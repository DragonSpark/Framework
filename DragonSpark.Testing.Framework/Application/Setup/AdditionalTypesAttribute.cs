using System;
using System.Collections.Immutable;

namespace DragonSpark.Testing.Framework.Application.Setup
{
	public class AdditionalTypesAttribute : TypeProviderAttributeBase
	{
		public AdditionalTypesAttribute( params Type[] additionalTypes ) : base( additionalTypes.ToImmutableArray() ) {}
	}
}
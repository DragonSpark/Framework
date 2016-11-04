using System;
using DragonSpark.TypeSystem;

namespace DragonSpark.Specifications
{
	public class AdapterInstanceSpecification : DelegatedSpecification<object>
	{
		public AdapterInstanceSpecification( params Type[] types ) : base( types.IsInstanceOfType ) {}
		public AdapterInstanceSpecification( params TypeAdapter[] types ) : base( types.IsInstanceOfType ) {}
	}
}
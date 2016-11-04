using DragonSpark.TypeSystem;
using System;

namespace DragonSpark.Specifications
{
	public class AdapterAssignableSpecification : DelegatedSpecification<Type>
	{
		public AdapterAssignableSpecification( params Type[] types ) : base( types.IsAssignableFrom ) {}
		public AdapterAssignableSpecification( params TypeAdapter[] types ) : base( types.IsAssignableFrom ) {}
	}
}
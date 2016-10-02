using DragonSpark.Specifications;
using DragonSpark.TypeSystem.Generics;
using System;
using System.Reflection;

namespace DragonSpark.TypeSystem.Metadata
{
	public struct Descriptor
	{
		public Descriptor( MethodInfo method ) : this( method, GenericMethodEqualitySpecification.Default.Get( method ).ToSpecificationDelegate() ) {}

		public Descriptor( MethodInfo method, Func<Type[], bool> specification )
		{
			Method = method;
			Specification = specification;
		}

		public MethodInfo Method { get; }
		public Func<Type[], bool> Specification { get; }
	}
}
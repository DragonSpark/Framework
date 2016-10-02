using System;

namespace DragonSpark.TypeSystem.Generics
{
	public struct GenericMethodCandidate<T>
	{
		public GenericMethodCandidate( T @delegate, Func<Type[], bool> specification )
		{
			Delegate = @delegate;
			Specification = specification;
		}

		public T Delegate { get; }
		public Func<Type[], bool> Specification { get; }
	}
}
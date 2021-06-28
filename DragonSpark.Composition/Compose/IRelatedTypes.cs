using DragonSpark.Model.Selection;
using DragonSpark.Runtime;
using System;
using System.Collections.Generic;

namespace DragonSpark.Composition.Compose
{
	public interface IRelatedTypes : ISelect<Type, List<Type>> {}

/*	public interface IRelatedTypes : ICommand<RelatedTypesResult> {}

	public readonly struct RelatedTypesResult
	{
		public RelatedTypesResult(List<Type> result, Type current)
		{
			Result  = result;
			Current = current;
		}

		public List<Type> Result { get; }

		public Type Current { get; }

		public void Deconstruct(out List<Type> result, out Type current)
		{
			result  = Result;
			current = Current;
		}
	}
*/
}
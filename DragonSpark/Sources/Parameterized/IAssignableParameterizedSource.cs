using System;

namespace DragonSpark.Sources.Parameterized
{
	public interface IAssignableParameterizedSource<TParameter, TResult> : IParameterizedSource<TParameter, TResult>, IAssignable<Func<TParameter, TResult>> {}
}
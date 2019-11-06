using System;
using DragonSpark.Model.Results;

namespace DragonSpark.Model.Commands
{
	public interface IDelegateAware<in T> : IResult<Action<T>> {}
}
using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Model.Commands
{
	public interface IDelegateAware<in T> : IResult<Action<T>> {}
}
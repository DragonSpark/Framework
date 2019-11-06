using System;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;

namespace DragonSpark.Runtime.Activation
{
	public interface IActivator<out T> : IResult<T> {}

	public interface IActivator : ISelect<Type, object> {}
}
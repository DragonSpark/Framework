using DragonSpark.Model.Operations;
using System;

namespace DragonSpark.Application.Runtime
{
	public interface IExceptionLogger : IOperation<(Type Owner, Exception Exception)> {}
}
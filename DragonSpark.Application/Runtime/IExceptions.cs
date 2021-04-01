using DragonSpark.Model.Operations;
using System;
using Exception = System.Exception;

namespace DragonSpark.Application.Runtime
{
	public interface IExceptions : IOperation<(Type Owner, Exception Exception)> {}
}
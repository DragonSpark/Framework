using DragonSpark.Model.Operations;
using System;
using Exception = System.Exception;

namespace DragonSpark.Application.Diagnostics
{
	public interface IExceptions : IOperation<(Type Owner, Exception Exception)> {}
}
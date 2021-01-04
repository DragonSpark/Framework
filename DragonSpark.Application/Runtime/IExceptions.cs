using DragonSpark.Model.Operations;
using System;

namespace DragonSpark.Application.Runtime
{
	public interface IExceptions : IOperation<(Type Owner, Exception Exception)> {}
}
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public class Delegating<T> : IOperation<T>
{
	readonly Func<IOperation<T>> _source;

	public Delegating(Func<IOperation<T>> source) => _source = source;

	public ValueTask Get(T parameter) => _source().Get(parameter);
}
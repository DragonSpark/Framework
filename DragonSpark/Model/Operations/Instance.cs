using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public class Instance<T> : IResulting<T>
{
	readonly T _instance;

	public Instance(T instance) => _instance = instance;

	public ValueTask<T> Get() => _instance.ToOperation();
}
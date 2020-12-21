using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations
{
	public class Resulting<T> : IResulting<T>
	{
		readonly T _instance;

		public Resulting(T instance) => _instance = instance;

		public ValueTask<T> Get() => _instance.ToOperation();
	}
}
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	class Class1 {}

	public interface ISave<in T> : IOperation<T> {}

	sealed class Save<T> : ISave<T>
	{
		readonly ISaveChanges<T> _save;

		public Save(ISaveChanges<T> save) => _save = save;

		public async ValueTask Get(T parameter) => await _save.Await(parameter);
	}
}
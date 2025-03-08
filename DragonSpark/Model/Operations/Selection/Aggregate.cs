using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Selection;

sealed class Aggregate<T> : ISelecting<Many<T>, T>
{
	public static Aggregate<T> Default { get; } = new();

	Aggregate() {}

	public async ValueTask<T> Get(Many<T> parameter)
	{
		var (alterations, seed) = parameter;
		var result = seed;
		foreach (var altering in alterations.Open())
		{
			result = await altering.Off(result);
		}
		return result;
	}
}
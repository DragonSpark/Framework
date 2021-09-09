using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations
{
	public readonly record struct Many<T>(Array<IAltering<T>> Alterations, T Seed);

	sealed class Aggregate<T> : ISelecting<Many<T>, T>
	{
		public static Aggregate<T> Default { get; } = new Aggregate<T>();

		Aggregate() {}

		public async ValueTask<T> Get(Many<T> parameter)
		{
			var (alterations, seed) = parameter;
			var result = seed;
			foreach (var altering in alterations.Open())
			{
				result = await altering.Await(result);
			}
			return result;
		}
	}
}
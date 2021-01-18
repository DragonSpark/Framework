using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations
{
	sealed class Alter<T> : ISelecting<(T Seed, Array<IAltering<T>> Alterations), T>
	{
		public static Alter<T> Default { get; } = new Alter<T>();

		Alter() {}

		public async ValueTask<T> Get((T Seed, Array<IAltering<T>> Alterations) parameter)
		{
			var (seed, alterations) = parameter;
			var result = seed;
			foreach (var altering in alterations.Open())
			{
				result = await altering.Await(result);
			}
			return result;
		}
	}
}
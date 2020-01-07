using DragonSpark.Compose;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Model.Selection.Alterations
{
	public class CompositeAlteration<T> : IAlteration<T>
	{
		readonly Array<IAlteration<T>> _alterations;

		public CompositeAlteration(params IAlteration<T>[] alterations) : this(alterations.Result()) {}

		public CompositeAlteration(Array<IAlteration<T>> alterations) => _alterations = alterations;

		public T Get(T parameter) => _alterations.Open().Alter(parameter);
	}
}
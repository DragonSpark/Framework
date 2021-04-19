using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations
{
	public class Inverse<T> : IDepending<T>
	{
		readonly Await<T, bool> _previous;

		public Inverse(ISelect<T, ValueTask<bool>> select) : this(select.Await) {}

		public Inverse(Await<T, bool> previous) => _previous = previous;

		public async ValueTask<bool> Get(T parameter)
		{
			var previous = await _previous(parameter);
			var result   = previous.Inverse();
			return result;
		}
	}
}
using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations
{
	sealed class Await<T> : ISelect<ValueTask<ValueTask<T>>, ValueTask<T>>
	{
		public static Await<T> Default { get; } = new Await<T>();

		Await() : this(false) {}

		readonly bool _capture;

		public Await(bool capture = false) => _capture = capture;

		public ValueTask<T> Get(ValueTask<ValueTask<T>> parameter)
			=> parameter.IsCompletedSuccessfully ? parameter.Result : Yield(parameter);

		async ValueTask<T> Yield(ValueTask<ValueTask<T>> parameter)
		{
			var outer  = await parameter.ConfigureAwait(_capture);
			var result = await outer.ConfigureAwait(_capture);
			return result;
		}
	}
}
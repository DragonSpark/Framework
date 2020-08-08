using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations
{
	public class Awaiting<T> : IOperation<T>
	{
		readonly Await<T> _await;

		public Awaiting(Await<T> await) => _await = @await;

		public async ValueTask Get(T parameter) => await _await(parameter);
	}

	sealed class Assuming<T> : ISelect<ValueTask<ValueTask<T>>, ValueTask<T>>
	{
		public static Assuming<T> Default { get; } = new Assuming<T>();

		Assuming() : this(false) {}

		readonly bool _capture;

		public Assuming(bool capture = false) => _capture = capture;

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
using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations
{
	sealed class Awaiting<T> : ISelect<ValueTask<ValueTask<T>>, ValueTask<T>>
	{
		public static Awaiting<T> Default { get; } = new Awaiting<T>();

		Awaiting() : this(false) {}

		readonly bool _capture;

		public Awaiting(bool capture = false) => _capture = capture;

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
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Results;

public sealed class TokenAdapter<T> : IToken<T>
{
	readonly IResulting<T> _previous;

	public TokenAdapter(IResulting<T> previous) => _previous = previous;

	public ValueTask<T> Get(CancellationToken parameter) => _previous.Get();
}
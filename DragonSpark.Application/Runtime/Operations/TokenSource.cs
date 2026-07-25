using DragonSpark.Model.Results;

namespace DragonSpark.Application.Runtime.Operations;

sealed class TokenSource : Stored<CancellationTokenSource>
{
	public TokenSource(IMutable<CancellationTokenSource?> store) : base(store, () => new()) {}
}
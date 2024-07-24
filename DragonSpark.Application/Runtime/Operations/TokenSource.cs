using DragonSpark.Model.Results;
using System.Threading;

namespace DragonSpark.Application.Runtime.Operations;

sealed class TokenSource : Stored<CancellationTokenSource>
{
	public TokenSource(IMutable<CancellationTokenSource?> store) : base(store, () => new()) {}
}
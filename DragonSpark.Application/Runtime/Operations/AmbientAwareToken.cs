using DragonSpark.Model.Operations;
using System.Threading;

namespace DragonSpark.Application.Runtime.Operations;

sealed class AmbientAwareToken : DragonSpark.Model.Results.Coalesce<CancellationToken>, IToken
{
	public AmbientAwareToken(IToken second) : base(AmbientToken.Default, second) {}
}
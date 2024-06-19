using DragonSpark.Model.Operations;
using System.Threading;

namespace DragonSpark.Application.Runtime.Operations;

sealed class AmbientAwareToken : DragonSpark.Model.Results.Coalesce<CancellationToken>, IScopedToken
{
	public AmbientAwareToken(IScopedToken second) : base(AmbientToken.Default, second) {}
}
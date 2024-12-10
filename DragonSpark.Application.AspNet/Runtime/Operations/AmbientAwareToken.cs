using System.Threading;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.AspNet.Runtime.Operations;

sealed class AmbientAwareToken : DragonSpark.Model.Results.Coalesce<CancellationToken>, IScopedToken
{
	public AmbientAwareToken(IScopedToken second) : base(AmbientToken.Default, second) {}
}

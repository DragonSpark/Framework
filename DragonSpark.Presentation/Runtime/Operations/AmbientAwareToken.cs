using System.Threading;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;

namespace DragonSpark.Presentation.Runtime.Operations;

sealed class AmbientAwareToken : CoalesceStructure<CancellationToken>, IRequestToken
{
	public AmbientAwareToken(IRequestToken second) : base(AmbientTokenOrNone.Default, second) {}
}
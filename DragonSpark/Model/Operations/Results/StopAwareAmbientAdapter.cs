using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Results;

public sealed class StopAwareAmbientAdapter<T> : Resulting<T>
{
	public StopAwareAmbientAdapter(ISelect<CancellationToken, ValueTask<T>> previous) 
		: this(previous, AmbientToken.Default) {}

	public StopAwareAmbientAdapter(ISelect<CancellationToken, ValueTask<T>> previous, IResult<CancellationToken> source)
		: base(previous.Then().Bind(source)) {}
}
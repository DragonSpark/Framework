using DragonSpark.Compose;
using DragonSpark.Diagnostics;
using DragonSpark.Model.Results;
using Polly;
using Policy = Polly.Policy;

namespace DragonSpark.Application.AspNet.Entities.Diagnostics;

public sealed class DurableConnectionPolicy : Deferred<IAsyncPolicy> // TODO: Rename DurableDataPolicy
{
	public static DurableConnectionPolicy Default { get; } = new();

	DurableConnectionPolicy() : this(IsDataException.Default.Get) {}

	DurableConnectionPolicy(Func<Exception, bool> data)
		: base(Policy.Handle(data).OrInner(data).Start().Select(DefaultRetryPolicy.Default)) {}
}
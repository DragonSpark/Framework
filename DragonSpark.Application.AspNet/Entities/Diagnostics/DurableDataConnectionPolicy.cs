using DragonSpark.Compose;
using DragonSpark.Diagnostics;
using DragonSpark.Model.Results;
using Polly;
using Policy = Polly.Policy;

namespace DragonSpark.Application.AspNet.Entities.Diagnostics;

public sealed class DurableDataConnectionPolicy : Deferred<IAsyncPolicy>
{
	public static DurableDataConnectionPolicy Default { get; } = new();

	DurableDataConnectionPolicy() : this(IsDataException.Default.Get) {}

	DurableDataConnectionPolicy(Func<Exception, bool> data)
		: base(Policy.Handle(data).OrInner(data).Start().Select(DefaultRetryPolicy.Default)) {}
}
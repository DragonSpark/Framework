using DragonSpark.Compose;
using DragonSpark.Diagnostics;
using DragonSpark.Model.Results;
using Polly;
using System.ClientModel;
using System.IO;
using Policy = Polly.Policy;

namespace DragonSpark.Azure.Ai;

public sealed class DurableConnectionPolicy : Deferred<IAsyncPolicy>
{
	public static DurableConnectionPolicy Default { get; } = new();

	DurableConnectionPolicy()
		: base(Start.A.Result(Policy.Handle<ClientResultException>().Or<IOException>())
		            .Select(DefaultRetryPolicy.Default)) {}
}
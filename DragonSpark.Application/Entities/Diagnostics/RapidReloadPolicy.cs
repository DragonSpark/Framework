using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;
using Polly;
using Policy = Polly.Policy;

namespace DragonSpark.Application.Entities.Diagnostics;

sealed class RapidReloadPolicy : Deferred<IAsyncPolicy>
{
	public static RapidReloadPolicy Default { get; } = new ();

	RapidReloadPolicy()
		: base(Policy.Handle<DbUpdateConcurrencyException>().Start().Select(RapidReloadPolicyBuilder.Default)) {}
}
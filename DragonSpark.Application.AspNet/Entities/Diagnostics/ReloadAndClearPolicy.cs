using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace DragonSpark.Application.Entities.Diagnostics;

sealed class ReloadAndClearPolicy : Deferred<IAsyncPolicy>
{
	public static ReloadAndClearPolicy Default { get; } = new ();

	ReloadAndClearPolicy()
		: base(Policy.Handle<DbUpdateConcurrencyException>().Start().Select(ReloadAndClearPolicyBuilder.Default)) {}
}
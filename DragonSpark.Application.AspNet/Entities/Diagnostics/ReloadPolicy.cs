using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace DragonSpark.Application.AspNet.Entities.Diagnostics;

sealed class ReloadPolicy : Deferred<IAsyncPolicy>
{
	public static ReloadPolicy Default { get; } = new ();

	ReloadPolicy()
		: base(Policy.Handle<DbUpdateConcurrencyException>().Start().Select(DefaultReloadPolicyBuilder.Default)) {}
}
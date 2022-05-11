using DragonSpark.Application.Model.Sequences;
using DragonSpark.Model.Sequences.Collections;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Claims.Compile;

sealed class ClaimsTransactional : Transactional<Claim>
{
	public static ClaimsTransactional Default { get; } = new();

	ClaimsTransactional() : base(new DelegatedEqualityComparer<Claim, string>(x => x.Type),
	                             x => x.Stored.Value != x.Input.Value) {}
}
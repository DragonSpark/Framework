using DragonSpark.Model.Sequences.Collections;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity
{
	sealed class ClaimsTransactional : Transactional<Claim>
	{
		public static ClaimsTransactional Default { get; } = new ClaimsTransactional();

		ClaimsTransactional() : base(new DelegatedEqualityComparer<Claim, string>(x => x.Type),
		                             x => x.Item1.Value != x.Item2.Value) {}
	}
}
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;

namespace DragonSpark.Application.Security.Identity.Claims.Compile;

sealed class ClaimNames : IEnumerable<string>
{
	readonly IKnownClaims _known;

	public ClaimNames(IKnownClaims known) => _known = known;

	[MustDisposeResource]
	public IEnumerator<string> GetEnumerator() => _known.Get().GetEnumerator();

	[MustDisposeResource]
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
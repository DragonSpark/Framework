using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace DragonSpark.Application.AspNet.Security.Identity.Claims.Compile;

sealed class ClaimNames : IEnumerable<string>
{
	readonly IKnownClaims _known;

	public ClaimNames(IKnownClaims known) => _known = known;

	[MustDisposeResource]
	public IEnumerator<string> GetEnumerator() => _known.Get().GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

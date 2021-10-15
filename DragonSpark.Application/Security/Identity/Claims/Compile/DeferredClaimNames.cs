using System;
using System.Collections;
using System.Collections.Generic;

namespace DragonSpark.Application.Security.Identity.Claims.Compile;

sealed class DeferredClaimNames : IEnumerable<string>
{
	readonly Func<IKnownClaims> _known;

	public DeferredClaimNames(Func<IKnownClaims> known) => _known = known;

	public IEnumerator<string> GetEnumerator() => _known().Get().GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
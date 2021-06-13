using System.Collections;
using System.Collections.Generic;

namespace DragonSpark.Application.Security.Identity.Model
{
	sealed class ClaimNames : IEnumerable<string>
	{
		readonly IKnownClaims _known;

		public ClaimNames(IKnownClaims known) => _known = known;

		public IEnumerator<string> GetEnumerator() => _known.Get().GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
using DragonSpark.Model;
using DragonSpark.Model.Sequences;
using System.Collections.Generic;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Claims.Compile
{
	public class ClaimExtractor : IExtractClaims
	{
		readonly IArray<string> _names;

		public ClaimExtractor(IEnumerable<string> names) : this(new DeferredArray<string>(names)) {}

		public ClaimExtractor(IArray<string> names) => _names = names;

		public Array<Claim> Get(ClaimsPrincipal parameter)
		{
			var names = _names.Get();
			var store = new List<Claim>();
			foreach (var name in names.Open())
			{
				if (parameter.HasClaim(name))
				{
					store.Add(parameter.FindFirst(name)!);
				}
			}

			var result = store.Count > 0 ? store.ToArray() : Empty.Array<Claim>();
			return result;
		}
	}
}
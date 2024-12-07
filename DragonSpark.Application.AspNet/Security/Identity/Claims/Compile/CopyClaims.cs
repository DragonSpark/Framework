using DragonSpark.Model.Commands;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Application.Security.Identity.Claims.Compile;

public class CopyClaims : ICommand<Transfer>
{
	readonly IExtractClaims _extract;

	protected CopyClaims(IEnumerable<string> names) : this(new ClaimExtractor(names)) {}

	protected CopyClaims(IExtractClaims extract) => _extract = extract;

	public void Execute(Transfer parameter)
	{
		var (from, to) = parameter;

		var destination = to.Identities.First();
		foreach (var claim in _extract.Get(from))
		{
			if (!to.HasClaim(claim.Type))
			{
				destination.AddClaim(claim);
			}
		}
	}
}
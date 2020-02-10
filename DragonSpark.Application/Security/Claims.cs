using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using Microsoft.AspNetCore.Identity;
using NetFabric.Hyperlinq;
using System.Linq;
using System.Security.Claims;

namespace DragonSpark.Application.Security
{
	sealed class Claims : IClaims
	{
		readonly Array<string> _keys;

		public Claims(params string[] keys) => _keys = keys;

		public Array<Claim> Get(ExternalLoginInfo parameter) => parameter.Principal.Claims
		                                                                 .Introduce(_keys.Open())
		                                                                 .Where(x => x.Item2.Contains(x.Item1.Type))
		                                                                 .Select(x => x.Item1)
		                                                                 .ToArray();
	}
}
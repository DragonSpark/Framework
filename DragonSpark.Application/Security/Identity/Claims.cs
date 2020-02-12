using DragonSpark.Model.Sequences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity
{
	public class Claims : IClaims
	{
		readonly Func<Claim, bool> _where;

		public Claims(Func<Claim, bool> where) => _where = @where;

		public Array<Claim> Get(IEnumerable<Claim> parameter) => parameter.Where(_where).ToArray();
	}
}
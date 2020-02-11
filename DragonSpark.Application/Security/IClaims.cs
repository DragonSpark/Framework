using DragonSpark.Model.Sequences;
using System.Collections.Generic;
using System.Security.Claims;

namespace DragonSpark.Application.Security
{
	public interface IClaims : IArray<IEnumerable<Claim>, Claim> {}
}
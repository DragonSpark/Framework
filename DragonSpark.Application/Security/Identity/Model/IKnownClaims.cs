using DragonSpark.Model.Results;
using System.Collections.Generic;

namespace DragonSpark.Application.Security.Identity.Model
{
	public interface IKnownClaims : IResult<IEnumerable<string>> {}
}
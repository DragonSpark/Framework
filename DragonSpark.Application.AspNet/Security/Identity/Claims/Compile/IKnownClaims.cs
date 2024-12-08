using DragonSpark.Model.Results;
using System.Collections.Generic;

namespace DragonSpark.Application.AspNet.Security.Identity.Claims.Compile;

public interface IKnownClaims : IResult<IEnumerable<string>>;
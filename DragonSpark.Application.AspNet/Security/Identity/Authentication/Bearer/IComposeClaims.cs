using System.Collections.Generic;
using System.Security.Claims;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Bearer;

public interface IComposeClaims<in T> : ISelect<T, IEnumerable<Claim>> {}
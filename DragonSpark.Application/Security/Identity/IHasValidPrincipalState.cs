using DragonSpark.Model.Operations.Selection.Conditions;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity;

public interface IHasValidPrincipalState : IDepending<ClaimsPrincipal> {}
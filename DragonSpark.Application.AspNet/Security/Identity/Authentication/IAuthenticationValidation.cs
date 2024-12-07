using DragonSpark.Model.Operations.Selection.Conditions;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Authentication;

public interface IAuthenticationValidation : IDepending<ClaimsPrincipal>;
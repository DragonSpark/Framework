using DragonSpark.Model.Operations.Selection.Conditions;
using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication;

public interface IAuthenticationValidation : IDepending<ClaimsPrincipal>;
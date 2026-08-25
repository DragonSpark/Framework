using DragonSpark.Model.Operations.Selection.Conditions;
using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Initialization;

public interface IValidateUser : IDepending<ClaimsPrincipal>;
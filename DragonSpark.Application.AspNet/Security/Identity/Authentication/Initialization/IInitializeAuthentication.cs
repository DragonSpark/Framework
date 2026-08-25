using DragonSpark.Model.Operations;
using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Initialization;

public interface IInitializeAuthentication : IOperation<ClaimsPrincipal>;
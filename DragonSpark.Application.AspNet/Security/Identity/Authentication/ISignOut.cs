using DragonSpark.Model.Operations;
using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication;

public interface ISignOut : IOperation<ClaimsPrincipal>;
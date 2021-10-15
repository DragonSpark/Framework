using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.Security.Identity.Authentication;

public interface IAuthentication : ISelecting<ExternalLoginInfo, SignInResult> {}
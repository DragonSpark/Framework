using DragonSpark.Model.Operations.Selection;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication;

public interface IAuthentication : ISelecting<ExternalLoginInfo, SignInResult>;
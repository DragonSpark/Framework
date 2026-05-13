using DragonSpark.Model.Operations.Selection.Stop;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Additional;

public interface IExternalLogin : IStopAware<ExternalLoginInfo, SignInResult>;
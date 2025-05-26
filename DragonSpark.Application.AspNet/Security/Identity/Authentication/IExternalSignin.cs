using DragonSpark.Model.Operations.Selection.Stop;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication;

public interface IExternalSignin : IStopAware<ExternalLoginInfo, SignInResult>;
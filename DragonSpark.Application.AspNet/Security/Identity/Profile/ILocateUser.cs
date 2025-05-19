using DragonSpark.Model.Operations.Selection.Stop;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Profile;

public interface ILocateUser<T> : IStopAware<ExternalLoginInfo, T?> where T : IdentityUser;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.Security.Identity.Profile;

public interface ILocateUser<T> : ISelecting<ExternalLoginInfo, T?> where T : IdentityUser {}
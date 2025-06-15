using DragonSpark.Model.Operations.Selection;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Profile;

public interface ILocateUser<T> : ISelecting<ExternalLoginInfo, T?> where T : IdentityUser;
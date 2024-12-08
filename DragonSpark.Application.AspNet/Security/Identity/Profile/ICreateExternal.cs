using DragonSpark.Model.Operations.Selection;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Profile;

public interface ICreateExternal<T> : ISelecting<ExternalLoginInfo, CreateUserResult<T>>;
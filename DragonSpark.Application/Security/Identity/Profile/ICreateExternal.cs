using DragonSpark.Model.Operations.Selection;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.Security.Identity.Profile;

public interface ICreateExternal<T> : ISelecting<ExternalLoginInfo, CreateUserResult<T>>;
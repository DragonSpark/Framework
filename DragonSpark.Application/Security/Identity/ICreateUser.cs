using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.Security.Identity
{
	public interface ICreateUser<T> : ISelecting<ExternalLoginInfo, CreateUserResult<T>> {}
}
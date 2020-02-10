using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.Security
{
	public interface IIdentityOperation<T> : IOperationResult<(ExternalLoginInfo Login, T User), IdentityResult> {}
}
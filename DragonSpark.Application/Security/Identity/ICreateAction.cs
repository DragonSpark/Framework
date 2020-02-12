using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.Security.Identity
{
	public interface ICreateAction : IOperationResult<ExternalLoginInfo, IdentityResult> {}
}
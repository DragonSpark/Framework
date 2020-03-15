using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.Security.Identity.Profile {
	interface IAuthenticationProfile : IOperationResult<ExternalLoginInfo> {}
}
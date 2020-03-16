using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.Security.Identity.Model {
	interface IAuthenticationProfile : IOperationResult<ExternalLoginInfo> {}
}
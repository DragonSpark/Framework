
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.Security.Identity
{
	public interface ICreateRequest : ISelecting<ExternalLoginInfo, IdentityResult> {}
}
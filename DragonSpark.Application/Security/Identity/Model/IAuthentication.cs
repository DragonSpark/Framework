using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.Security.Identity.Model
{
	public interface IAuthentication : ISelecting<ExternalLoginInfo, SignInResult> {}
}
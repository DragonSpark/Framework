using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.Security.Identity.Authentication
{
	public interface IExternalSignin : ISelecting<ExternalLoginInfo, SignInResult> {}
}
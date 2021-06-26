using DragonSpark.Model.Commands;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Model
{
	public interface IClearAuthenticationState : ICommand<ClaimsPrincipal> {}
}
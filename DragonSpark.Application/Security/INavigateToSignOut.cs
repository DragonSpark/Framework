using DragonSpark.Model.Commands;
using System.Security.Claims;

namespace DragonSpark.Application.Security
{
	public interface INavigateToSignOut : ICommand<ClaimsPrincipal> {}
}
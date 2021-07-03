using DragonSpark.Model.Commands;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Model
{
	public sealed class ClearCurrentAuthenticationState : DelegatedParameterCommand<ClaimsPrincipal>
	{
		public ClearCurrentAuthenticationState(IClearAuthenticationState command, ICurrentPrincipal parameter)
			: base(command, parameter) {}
	}
}
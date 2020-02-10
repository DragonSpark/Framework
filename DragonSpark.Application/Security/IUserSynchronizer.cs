using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DragonSpark.Application.Security
{
	public interface IUserSynchronizer<T> : IOperationResult<(ClaimsPrincipal Source, Destination<T> Destination), bool>
		where T : IdentityUser {}
}
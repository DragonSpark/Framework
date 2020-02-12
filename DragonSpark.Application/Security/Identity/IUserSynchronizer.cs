using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity
{
	public interface IUserSynchronizer<T> : IOperationResult<(Stored<T> Stored, ClaimsPrincipal Source), bool>
		where T : IdentityUser {}
}
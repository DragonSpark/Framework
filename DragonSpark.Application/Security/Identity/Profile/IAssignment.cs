using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Profile {
	public interface IAssignment<T> : ICondition<Claim>, ICommand<(T User, string Value)>
		where T : IdentityUser {}
}
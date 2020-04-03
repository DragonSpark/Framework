using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Profile
{
	public interface IAccessor<T> : ICondition<Claim>, ICommand<(T User, string Value)>, ISelect<T, string>
		where T : IdentityUser
	{
		string Name { get; }
	}
}
using DragonSpark.Model.Selection;
using System.Security.Claims;

namespace DragonSpark.Application.Security
{
	public interface IRequiredClaim : ISelect<ClaimsPrincipal, string> {}
}
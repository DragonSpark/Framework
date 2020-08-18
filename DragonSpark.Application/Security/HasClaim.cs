using DragonSpark.Model.Selection.Conditions;
using System.Security.Claims;

namespace DragonSpark.Application.Security
{
	public class HasClaim : ICondition<ClaimsPrincipal>
	{
		readonly string _claim;
		readonly string _value;

		public HasClaim(string claim) : this(claim, bool.TrueString) {}

		public HasClaim(string claim, string value)
		{
			_claim = claim;
			_value = value;
		}

		public bool Get(ClaimsPrincipal parameter) => parameter.HasClaim(_claim, _value);
	}
}
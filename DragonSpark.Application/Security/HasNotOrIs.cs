using DragonSpark.Model.Selection.Conditions;
using System.Security.Claims;

namespace DragonSpark.Application.Security
{
	public class HasNotOrIs : ICondition<ClaimsPrincipal>
	{
		readonly string _type;
		readonly string _value;

		public HasNotOrIs(string type, string value)
		{
			_type  = type;
			_value = value;
		}

		public bool Get(ClaimsPrincipal parameter)
		{
			var claim  = parameter.FindFirst(_type);
			var result = claim == null || claim.Value == _value;
			return result;
		}
	}
}
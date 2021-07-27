using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;

namespace DragonSpark.Application.Security.Identity.Claims
{
	public class SubKeyClaimAction : IClaimAction
	{
		readonly string _claim, _type;
		readonly SubKey _key;

		public SubKeyClaimAction(string claim, SubKey key, string type = "string")
		{
			_key   = key;
			_claim = claim;
			_type  = type;
		}

		public void Execute(ClaimActionCollection parameter)
		{
			parameter.MapJsonSubKey(_claim, _key.Key, _key.Element, _type);
		}
	}
}
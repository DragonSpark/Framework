using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.Security
{
	sealed class UniqueId : ISelect<ExternalLoginInfo, string>
	{
		public static UniqueId Default { get; } = new UniqueId();

		UniqueId() {}

		public string Get(ExternalLoginInfo parameter) => $"{parameter.LoginProvider}+{parameter.ProviderKey}";
	}
}
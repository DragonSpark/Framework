using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DragonSpark.Application.Testing.Objects
{
	sealed class NewUser : ISelect<ExternalLoginInfo, User>
	{
		public static NewUser Default { get; } = new NewUser();

		NewUser() {}

		public User Get(ExternalLoginInfo parameter)
		{
			var result = new User
			{
				Id       = parameter.UniqueId(),
				UserName = parameter.Principal.FindFirstValue(ClaimTypes.Name)
			};
			return result;
		}
	}
}
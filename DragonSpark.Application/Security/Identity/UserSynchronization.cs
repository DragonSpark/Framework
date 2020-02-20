﻿using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity
{
	sealed class UserSynchronization<T> : IUserSynchronization where T : IdentityUser
	{
		readonly SignInManager<T>     _authentication;
		readonly IUserSynchronizer<T> _synchronizer;
		readonly UserManager<T>       _users;

		public UserSynchronization(SignInManager<T> authentication, UserManager<T> users,
		                           IUserSynchronizer<T> synchronizer)
		{
			_authentication = authentication;
			_synchronizer   = synchronizer;
			_users          = users;
		}

		public async ValueTask Get(ExternalLoginInfo parameter)
		{
			var user      = await _users.GetUser(parameter);
			var principal = await _authentication.CreateUserPrincipalAsync(user);

			if (await _synchronizer.Get((new Stored<T>(user, principal), parameter.Principal)))
			{
				await _authentication.RefreshSignInAsync(user);
			}
		}
	}
}
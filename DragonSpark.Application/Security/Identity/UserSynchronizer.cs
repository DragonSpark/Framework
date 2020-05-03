﻿using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity
{
	class UserSynchronizer<T> : IUserSynchronizer<T> where T : IdentityUser
	{
		readonly IUserSynchronizer<T> _previous;
		readonly IMarkModified<T>     _modified;

		public UserSynchronizer(UserClaimSynchronizer<T> previous, IMarkModified<T> modified)
		{
			_previous = previous;
			_modified = modified;
		}

		public async ValueTask<bool> Get(Synchronization<T> parameter)
		{
			var result = await _previous.Get(parameter).ConfigureAwait(false);
			if (result)
			{
				await _modified.Get(parameter.Profile.Profile).ConfigureAwait(false);
			}
			return result;
		}
	}
}
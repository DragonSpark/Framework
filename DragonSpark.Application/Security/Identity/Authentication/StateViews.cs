﻿using DragonSpark.Composition;
using JetBrains.Annotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication
{
	sealed class StateViews<T> : IStateViews<T> where T : class
	{
		readonly IUsers<T>    _users;
		readonly StateView<T> _default;

		[UsedImplicitly]
		public StateViews(IUsers<T> users) : this(users, StateView<T>.Default) {}

		[Candidate(false)]
		public StateViews(IUsers<T> users, StateView<T> @default)
		{
			_users   = users;
			_default = @default;
		}

		public async ValueTask<StateView<T>> Get(ClaimsPrincipal parameter)
		{
			await using var users   = _users.Get();
			var             manager = users.Subject;
			var             user    = await manager.GetUserAsync(parameter);
			var result = user != null
				             ? new StateView<T>(new AuthenticationState<T>(parameter, user),
				                                manager.SupportsUserSecurityStamp
					                                ? await manager.GetSecurityStampAsync(user) ?? string.Empty
					                                : null)
				             : _default;

			return result;
		}
	}
}
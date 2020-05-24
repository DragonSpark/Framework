using DragonSpark.Compose;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity
{
	sealed class Adapters<T> : IAdapters where T : class
	{
		readonly IStateViews<T>     _views;
		readonly INavigateToSignOut _exit;

		[UsedImplicitly]
		public Adapters(IStateViews<T> views, INavigateToSignOut exit)
		{
			_views = views;
			_exit  = exit;
		}

		public async Task<AuthenticationState> Get(Task<AuthenticationState> parameter)
		{
			var previous = await parameter.ConfigureAwait(false);
			var view     = await _views.Await(previous.User);

			if (previous.User.Identity.IsAuthenticated && view.State.Profile == null)
			{
				_exit.Execute();
			}

			var result = view.State;
			return result;
		}
	}
}
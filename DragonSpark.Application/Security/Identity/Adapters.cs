using DragonSpark.Compose;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity
{
	sealed class Adapters<T> : IAdapters where T : class
	{
		readonly IStateViews<T> _views;
		readonly NavigationManager _navigation;

		[UsedImplicitly]
		public Adapters(IStateViews<T> views, NavigationManager navigation)
		{
			_views = views;
			_navigation = navigation;
		}

		public async Task<AuthenticationState> Get(Task<AuthenticationState> parameter)
		{
			var previous = await parameter.ConfigureAwait(false);
			var view     = await _views.Await(previous.User);

			if (previous.User.Identity.IsAuthenticated && view.State.Profile == null)
			{
				_navigation.NavigateTo("/Identity/Account/LogOut", true); // TODO: RedirectToLogin
			}
			var result   = view.State;
			return result;
		}
	}
}
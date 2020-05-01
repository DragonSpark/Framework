using JetBrains.Annotations;
using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity
{
	sealed class Adapters<T> : IAdapters where T : class
	{
		readonly IStateViews<T> _views;

		[UsedImplicitly]
		public Adapters(IStateViews<T> views) => _views = views;

		public async Task<AuthenticationState> Get(Task<AuthenticationState> parameter)
		{
			var previous = await parameter.ConfigureAwait(false);
			var view     = await _views.Get(previous.User);
			var result   = view.State;
			return result;
		}
	}
}
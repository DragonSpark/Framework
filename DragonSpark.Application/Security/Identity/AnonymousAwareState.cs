using DragonSpark.Compose;
using JetBrains.Annotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity
{
	sealed class AnonymousAwareState<T> : IStateViews<T> where T : class
	{
		readonly IStateViews<T> _views;
		readonly StateView<T>   _default;

		[UsedImplicitly]
		public AnonymousAwareState(IStateViews<T> views) : this(views, StateView<T>.Default) {}

		public AnonymousAwareState(IStateViews<T> views, StateView<T> @default)
		{
			_views   = views;
			_default = @default;
		}

		public ValueTask<StateView<T>> Get(ClaimsPrincipal parameter)
			=> parameter.Identity?.IsAuthenticated ?? false ? _views.Get(parameter) : _default.ToOperation();
	}
}
using DragonSpark.Application.AspNet.Security;
using DragonSpark.Application.Communication.Http.Security;
using DragonSpark.Application.Security.Identity.Bearer;
using DragonSpark.Compose;
using DragonSpark.Runtime;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Communication.Http;

sealed class ComposeAccessTokenView : IAccessTokenStore
{
	readonly ICurrentContext            _context;
	readonly ApplicationUserAwareBearer _bearer;
	readonly ITime                      _time;

	public ComposeAccessTokenView(ICurrentContext context, ApplicationUserAwareBearer bearer)
		: this(context, bearer, Time.Default) {}

	public ComposeAccessTokenView(ICurrentContext context, ApplicationUserAwareBearer bearer, ITime time)
	{
		_context = context;
		_bearer  = bearer;
		_time    = time;
	}

	public ValueTask<AccessTokenView?> Get(CancellationToken parameter)
	{
		var current = _context.Get().User;
		var number  = current.Number();
		if (number is not null)
		{
			var access = _bearer.Get(current).Verify();
			return new AccessTokenView(number.Value.ToString(), _time.Get(), new(access, string.Empty, 3600))
				.ToOperation<AccessTokenView?>();
		}

		return default(AccessTokenView?).ToOperation();
	}
}
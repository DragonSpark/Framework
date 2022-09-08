using DragonSpark.Application.Security;
using DragonSpark.Compose;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace DragonSpark.Application.Diagnostics;

sealed class ContextAwareExceptions : IExceptions
{
	readonly IExceptions          _previous;
	readonly ICurrentContext      _context;
	readonly IHttpContextAccessor _accessor;

	public ContextAwareExceptions(IExceptions previous, ICurrentContext context, IHttpContextAccessor accessor)
	{
		_previous = previous;
		_context  = context;
		_accessor = accessor;
	}

	public async ValueTask Get(ExceptionInput parameter)
	{
		var existing = _accessor.HttpContext;
		_accessor.HttpContext = _context.Get();
		await _previous.Await(parameter);
		_accessor.HttpContext = existing;
	}
}
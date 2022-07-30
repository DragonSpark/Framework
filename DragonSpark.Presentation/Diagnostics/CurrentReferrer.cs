using DragonSpark.Application.Communication;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Diagnostics;

sealed class CurrentReferrer : IResult<string?>
{
	readonly IHttpContextAccessor _context;
	readonly IHeader              _header;

	public CurrentReferrer(IHttpContextAccessor context) : this(context, RefererHeader.Default) {}

	public CurrentReferrer(IHttpContextAccessor context, IHeader header)
	{
		_context = context;
		_header  = header;
	}

	public string? Get()
	{
		var context = _context.HttpContext;
		var result  = context is not null ? _header.Get(context.Request.Headers) : default;
		return result;
	}
}
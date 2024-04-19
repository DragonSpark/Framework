using DragonSpark.Application.Security.Identity.Bearer;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using System;
using System.Net.Http;

namespace DragonSpark.Application.Communication;

public abstract class Configure : ICommand<HttpClient>
{
	readonly Uri              _base;
	readonly IResult<string?> _bearer;

	protected Configure(IResult<Uri> connection) : this(connection.Get(), AmbientBearer.Default) {}

	protected Configure(Uri @base, IResult<string?> bearer)
	{
		_base   = @base;
		_bearer = bearer;
	}

	public void Execute(HttpClient parameter)
	{
		var bearer  = _bearer.Get();
		var headers = parameter.DefaultRequestHeaders;
		headers.Authorization = bearer is not null ? new("Bearer", bearer) : headers.Authorization;
		parameter.BaseAddress = _base;
	}
}
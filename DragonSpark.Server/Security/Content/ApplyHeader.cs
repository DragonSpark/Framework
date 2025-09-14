using DragonSpark.Compose;
using DragonSpark.Model.Operations.Allocated;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Server.Security.Content;

public class ApplyHeader : IAllocated<HttpContext>
{
	readonly RequestDelegate           _next;
	readonly string                    _name;
	readonly Func<HttpContext, string> _value;

	protected ApplyHeader(RequestDelegate next, string name, string value) : this(next, name, value.Accept) {}

	protected ApplyHeader(RequestDelegate next, string name, Func<HttpContext, string> value)
	{
		_next  = next;
		_name  = name;
		_value = value;
	}

	public Task Get(HttpContext parameter)
	{
		parameter.Response.Headers[_name] = _value(parameter);
		return _next(parameter);
	}
}
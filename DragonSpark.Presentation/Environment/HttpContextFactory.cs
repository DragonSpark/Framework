using DragonSpark.Model.Results;
using DragonSpark.Presentation.Connections.Circuits;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace DragonSpark.Presentation.Environment;

sealed class HttpContextFactory : IHttpContextFactory
{
	readonly IHttpContextFactory                 _previous;
	readonly IMutable<IMutable<CircuitRecord?>?> _context;
	readonly string                              _path;

	public HttpContextFactory(IHttpContextFactory previous) : this(previous, CurrentCircuitStore.Default) {}

	public HttpContextFactory(IHttpContextFactory previous, IMutable<IMutable<CircuitRecord?>?> context,
	                          string path = "/_blazor") // config?
	{
		_previous = previous;
		_context  = context;
		_path     = path;
	}

	public HttpContext Create(IFeatureCollection featureCollection)
	{
		var result = _previous.Create(featureCollection);
		var path   = result.Request.Path;
		if (path.HasValue && path.Value.StartsWith(_path))
		{
			_context.Execute(new Variable<CircuitRecord>());
		}

		return result;
	}

	public void Dispose(HttpContext httpContext)
	{
		_context.Execute(default);
		_previous.Dispose(httpContext);
	}
}
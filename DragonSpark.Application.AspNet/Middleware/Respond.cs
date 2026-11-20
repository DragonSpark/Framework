using Microsoft.AspNetCore.Diagnostics;

namespace DragonSpark.Application.AspNet.Middleware;

public sealed class Respond : IRespond
{
	public static Respond Default { get; } = new();

	Respond() {}

	public Task Get(RespondInput parameter)
	{
		var (context, code) = parameter;
		if (context.Features.Get<IStatusCodePagesFeature>() is {} feature)
		{
			feature.Enabled = false;
		}

		context.Response.StatusCode = code;
		return Task.CompletedTask;
	}
}
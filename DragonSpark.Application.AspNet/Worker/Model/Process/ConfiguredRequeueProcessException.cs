using System;

namespace DragonSpark.Application.AspNet.Worker.Model.Process;

public sealed class ConfiguredRequeueProcessException : RequeueProcessException
{
	public ConfiguredRequeueProcessException(string reason, TimeSpan? visibility = null, TimeSpan? life = null)
		: base(reason)
	{
		Visibility = visibility;
		Life       = life;
	}

	public TimeSpan? Visibility { get; }

	public TimeSpan? Life { get; }
}
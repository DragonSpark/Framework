using System;

namespace DragonSpark.Application.AspNet.Security.Identity.Bearer;

public sealed class BearerSettings
{
	public string Key { get; set; } = null!;

	public string Issuer { get; set; } = null!;

	public string Audience { get; set; } = null!;

	public TimeSpan Window { get; set; } = TimeSpan.FromHours(1);
}
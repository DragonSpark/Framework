using System;

namespace DragonSpark.Application.Security.Identity.Bearer;

public sealed class BearerSettings
{
	public string Key { get; set; } = default!;

	public string Issuer { get; set; } = default!;

	public string Audience { get; set; } = default!;

	public TimeSpan Window { get; set; } = TimeSpan.FromHours(1);
}
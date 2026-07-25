using System;

namespace DragonSpark.Application.AspNet.Workers;

public sealed class ProcessNotification
{
	public Guid Id { get; set; }

	public required string Destination { get; set; }

	public required ExternalProcess Subject { get; set; }

	public required DateTimeOffset Created { get; set; }

	public required DateTimeOffset? AvailableAt { get; set; }

	public required TimeSpan? Lifetime { get; set; }
}
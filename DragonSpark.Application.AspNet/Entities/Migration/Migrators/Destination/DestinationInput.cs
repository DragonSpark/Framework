using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;

public sealed record DestinationInput<T>(
	ILogger Logger,
	DbContext Source,
	DbContext Destination,
	Array<T> From,
	uint Total);
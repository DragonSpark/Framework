using DragonSpark.Contracts.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public sealed record BatchInput<T>(
	ILogger Logger,
	DbContext Source,
	DbContext Destination,
	IQueryable<T> From,
	Partition Partition,
	uint Total);
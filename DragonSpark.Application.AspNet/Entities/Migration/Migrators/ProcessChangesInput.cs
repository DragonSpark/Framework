using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public sealed record ProcessChangesInput<T>(
	ILogger Logger,
	ushort PageSize,
	DbContext Source,
	DbContext Destination,
	IQueryable<T> From,
	uint Total);
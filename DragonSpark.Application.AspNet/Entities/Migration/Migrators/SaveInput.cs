using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public readonly record struct SaveInput<T>(
	ILogger Logger,
	ushort PageSize,
	DbContext Destination,
	IQueryable<T> Entities,
	uint Total)
	where T : class;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public readonly record struct SaveInput<T>(
	ILogger Logger,
	ushort PageSize,
	DbContext Destination,
	Array<T> Entities,
	uint Total)
	where T : class;
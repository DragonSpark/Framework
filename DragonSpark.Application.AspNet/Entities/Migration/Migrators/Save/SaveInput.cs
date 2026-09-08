using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Save;

public readonly record struct SaveInput(
	ILogger Logger,
	ushort PageSize,
	DbContext Destination,
	uint Total);
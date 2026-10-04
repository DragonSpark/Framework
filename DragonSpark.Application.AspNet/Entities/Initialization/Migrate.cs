using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Initialization;

public sealed class Migrate : IInitialize
{
	public static Migrate Default { get; } = new();

	Migrate() {}

	public ValueTask Get(Stop<DbContext> parameter)
	{
		var (subject, stop) = parameter;
		return subject.Database.MigrateAsync(stop).ToOperation();
	}
}
using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Initialization;

public sealed class Migrate : IInitialize
{
	public static Migrate Default { get; } = new();

	Migrate() {}

	public ValueTask Get(DbContext parameter) => parameter.Database.MigrateAsync().ToOperation();
}
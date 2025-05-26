using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Initialization;

public sealed class Migrate : IInitialize
{
	public static Migrate Default { get; } = new();

	Migrate() {}

	public ValueTask Get(Stop<DbContext> parameter) => parameter.Subject.Database.MigrateAsync(parameter).ToOperation();
}
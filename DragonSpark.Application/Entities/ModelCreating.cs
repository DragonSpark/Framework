using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities
{
	public readonly record struct ModelCreating(DbContext Context, ModelBuilder Builder);
}
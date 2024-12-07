using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities.Initialization;

public readonly record struct ModelCreating(DbContext Context, ModelBuilder Builder);
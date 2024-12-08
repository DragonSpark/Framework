using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Initialization;

public readonly record struct ModelCreating(DbContext Context, ModelBuilder Builder);
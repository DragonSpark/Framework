using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Initialization;

public readonly record struct SeedInput(IServiceProvider Services, DbContext Context);
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Initialization;

public readonly record struct InitializeInput(IServiceProvider Services, DbContext Subject);
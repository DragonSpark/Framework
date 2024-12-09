using Microsoft.EntityFrameworkCore;
using System;

namespace DragonSpark.Application.AspNet.Entities.Initialization;

public readonly record struct SeedInput(IServiceProvider Services, DbContext Context);
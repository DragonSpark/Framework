using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public readonly record struct EmitContextInput(string Label, DbContext Context);
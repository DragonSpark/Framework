using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public readonly record struct ApplyInput<T>(DbContext Context, EntityEntry<T> Entry) where T : class;
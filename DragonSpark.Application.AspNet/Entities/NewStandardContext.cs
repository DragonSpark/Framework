using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities;

public sealed class NewStandardContext<T> : Result<DbContext>, INewContext where T : DbContext
{
    public NewStandardContext(IDbContextFactory<T> factory) : base(factory.CreateDbContext) {}
}
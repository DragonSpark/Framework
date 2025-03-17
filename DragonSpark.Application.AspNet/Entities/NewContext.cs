using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities;

public class NewContext<T> : Result<T>, INewContext<T> where T : DbContext
{
	public NewContext(IDbContextFactory<T> factory) : base(factory.CreateDbContext) {}
}
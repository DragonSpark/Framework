using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities;

public sealed class FixedContexts : Instance<DbContext>, IContexts
{
	public FixedContexts(DbContext context) : base(context) {}
}
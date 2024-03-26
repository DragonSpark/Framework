using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities;

public sealed class FixedContexts : Instance<Scope>, IContexts
{
	public FixedContexts(DbContext context) : base(new (context, context)) {}
}
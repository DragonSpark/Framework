using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities;

public class InstanceScopes : DragonSpark.Model.Results.Instance<Scope>, IScopes
{
	protected InstanceScopes(DbContext context, IBoundary boundary) : base(new(context, boundary)) {}
}
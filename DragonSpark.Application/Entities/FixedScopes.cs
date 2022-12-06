using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities;

public sealed class FixedScopes : InstanceScopes
{
	public FixedScopes(DbContext context) : base(context, EmptyBoundary.Default) {}
}
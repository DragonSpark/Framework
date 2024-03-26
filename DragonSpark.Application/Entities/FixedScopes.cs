using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities;

public sealed class FixedScopes : Instance<DbContext>, IScopes
{
	public FixedScopes(DbContext context) : base(context) {}
}
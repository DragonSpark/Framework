using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities;

sealed class SessionScopes : InstanceScopes, ISessionScopes
{
	public SessionScopes(DbContext context, AmbientAwareInstanceBoundary boundary) : base(context, boundary) {}
}
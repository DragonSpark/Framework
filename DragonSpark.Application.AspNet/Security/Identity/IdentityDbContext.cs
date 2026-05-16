using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Security.Identity;

[MustDisposeResource]
public class IdentityDbContext<T>
	: Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext<T, IdentityRole<uint>, uint>
	where T : IdentityUser
{
	public IdentityDbContext() {}

	public IdentityDbContext(DbContextOptions options) : base(options) {}
}

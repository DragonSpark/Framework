using DragonSpark.Application.AspNet.Security.Identity;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Testing.Objects;

sealed class ApplicationStorage : IdentityDbContext<User>
{
	public ApplicationStorage(DbContextOptions<ApplicationStorage> options) : base(options) {}
}
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Testing.Objects
{
	sealed class ApplicationStorage : Application.Security.Identity.IdentityDbContext<User>
	{
		public ApplicationStorage(DbContextOptions<ApplicationStorage> options) : base(options) {}
	}
}
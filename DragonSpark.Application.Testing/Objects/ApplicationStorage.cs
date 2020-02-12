using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Testing.Objects {
	sealed class ApplicationStorage : IdentityDbContext<User>
	{
		public ApplicationStorage(DbContextOptions<ApplicationStorage> options) : base(options) {}
	}
}
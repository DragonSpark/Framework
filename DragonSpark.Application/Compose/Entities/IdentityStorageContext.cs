using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Compose.Entities
{
	public sealed class IdentityStorageContext<T, TContext> where TContext : DbContext where T : class
	{
		readonly ApplicationProfileContext _subject;

		public IdentityStorageContext(ApplicationProfileContext subject) => _subject = subject;

		public IdentityStorageConfigurationContext<T, TContext> Using => new(_subject);
	}
}
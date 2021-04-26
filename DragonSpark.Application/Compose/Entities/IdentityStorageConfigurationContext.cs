using DragonSpark.Model.Selection.Alterations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Compose.Entities
{
	public sealed class IdentityStorageConfigurationContext<T, TContext> where TContext : DbContext where T : class
	{
		readonly ApplicationProfileContext _subject;

		public IdentityStorageConfigurationContext(ApplicationProfileContext subject) => _subject = subject;

		public ApplicationProfileContext SqlServer() => Configuration(SqlStorageConfiguration<TContext>.Default);

		public ApplicationProfileContext Configuration(Alter<StorageConfigurationBuilder> configuration,
		                                               ServiceLifetime lifetime = ServiceLifetime.Scoped)
			=> Configuration(configuration(new StorageConfigurationBuilder()).Get(), lifetime);

		public ApplicationProfileContext Configuration(IStorageConfiguration configuration,
		                                               ServiceLifetime lifetime = ServiceLifetime.Scoped)
			=> _subject.Then(new ConfigureStorage<T, TContext>(configuration, lifetime))
			           .Configure(Initialize<TContext>.Default.Get);
	}
}
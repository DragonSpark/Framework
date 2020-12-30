using DragonSpark.Model.Selection.Alterations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Compose.Entities
{
	public sealed class IdentityStorageConfigurationContext<T, TContext> where TContext : DbContext where T : class
	{
		readonly ApplicationProfileContext _subject;

		public IdentityStorageConfigurationContext(ApplicationProfileContext subject) => _subject = subject;

		public ApplicationProfileContext SqlServer() => Configuration(SqlStorageConfiguration<TContext>.Default);

		public ApplicationProfileContext Configuration(Alter<StorageConfigurationBuilder> configuration)
			=> Configuration(configuration(new StorageConfigurationBuilder()).Get());

		public ApplicationProfileContext Configuration(IStorageConfiguration configuration)
			=> _subject.Then(new ConfigureStorage<T, TContext>(configuration))
			           .Configure(Initialize<TContext>.Default.Get);
	}
}
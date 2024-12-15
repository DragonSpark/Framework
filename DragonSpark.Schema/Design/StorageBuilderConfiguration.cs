using DragonSpark.Application.AspNet.Entities.Configure;
using DragonSpark.Compose;

namespace DragonSpark.Schema.Design
{
	sealed class StorageBuilderConfiguration : StorageBuilderConfiguration<ApplicationState>
	{
		public static StorageBuilderConfiguration Default { get; } = new StorageBuilderConfiguration();

		StorageBuilderConfiguration() : base(A.Type<StorageBuilder>(), KnownSchemaModifications.Default) {}
	}
}
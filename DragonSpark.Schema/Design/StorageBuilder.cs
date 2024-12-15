using DragonSpark.Application.AspNet.Entities.Design;
using JetBrains.Annotations;

namespace DragonSpark.Schema.Design
{
	[UsedImplicitly]
	sealed class StorageBuilder : StorageBuilder<ApplicationState>
	{
		public StorageBuilder() : base(StorageBuilderConfiguration.Default.Execute) {}
	}
}
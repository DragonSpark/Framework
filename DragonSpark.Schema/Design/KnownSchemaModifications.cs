using DragonSpark.Application.AspNet.Entities.Initialization;

namespace DragonSpark.Schema.Design
{
	public sealed class KnownSchemaModifications : SchemaModification
	{
		public static KnownSchemaModifications Default { get; } = new KnownSchemaModifications();

		KnownSchemaModifications() {}
	}
}
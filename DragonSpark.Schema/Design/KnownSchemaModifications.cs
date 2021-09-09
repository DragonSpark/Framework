using DragonSpark.Application.Entities;

namespace DragonSpark.Schema.Design
{
	public sealed class KnownSchemaModifications : SchemaModification
	{
		public static KnownSchemaModifications Default { get; } = new KnownSchemaModifications();

		KnownSchemaModifications() : base() {}
	}
}
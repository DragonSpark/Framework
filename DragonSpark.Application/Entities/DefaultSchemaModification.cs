namespace DragonSpark.Application.Entities
{
	public sealed class DefaultSchemaModification : SchemaModification
	{
		public static DefaultSchemaModification Default { get; } = new DefaultSchemaModification();

		DefaultSchemaModification() {}
	}
}
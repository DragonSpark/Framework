namespace DragonSpark.Application.Entities.Initialization;

public sealed class DefaultSchemaModification : SchemaModification
{
	public static DefaultSchemaModification Default { get; } = new DefaultSchemaModification();

	DefaultSchemaModification() {}
}
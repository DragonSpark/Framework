namespace DragonSpark.Application.AspNet.Entities.Search;

public sealed class DefaultCatalogName : Text.Text
{
	public static DefaultCatalogName Default { get; } = new();

	DefaultCatalogName() : base("DefaultFullTextCatalog") {}
}
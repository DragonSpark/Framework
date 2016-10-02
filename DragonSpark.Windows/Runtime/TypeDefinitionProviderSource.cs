namespace DragonSpark.Windows.Runtime
{
	public sealed class TypeDefinitionProviderSource : TypeSystem.Metadata.TypeDefinitionProviderSource
	{
		public new static TypeDefinitionProviderSource Default { get; } = new TypeDefinitionProviderSource();
		TypeDefinitionProviderSource() : base( MetadataTypeDefinitionProvider.Default ) {}
	}
}
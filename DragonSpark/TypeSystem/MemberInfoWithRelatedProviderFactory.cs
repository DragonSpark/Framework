namespace DragonSpark.TypeSystem
{
	class MemberInfoWithRelatedProviderFactory : MemberInfoProviderFactoryBase
	{
		public static MemberInfoWithRelatedProviderFactory Instance { get; } = new MemberInfoWithRelatedProviderFactory();

		public MemberInfoWithRelatedProviderFactory() : this( MemberInfoAttributeProviderFactory.Instance ) {}

		public MemberInfoWithRelatedProviderFactory( MemberInfoAttributeProviderFactory inner ) : base( inner, true ) {}
	}
}
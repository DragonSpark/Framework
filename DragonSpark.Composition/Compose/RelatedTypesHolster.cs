namespace DragonSpark.Composition.Compose
{
	public sealed class RelatedTypesHolster
	{
		public static RelatedTypesHolster Default { get; } = new RelatedTypesHolster();

		RelatedTypesHolster() : this(ServiceTypes.Default, Dependencies.Default) {}

		public RelatedTypesHolster(IServiceTypes none, Dependencies dependencies)
		{
			None         = none;
			Dependencies = dependencies;
		}

		public IServiceTypes None { get; }

		public Dependencies Dependencies { get; }
	}
}
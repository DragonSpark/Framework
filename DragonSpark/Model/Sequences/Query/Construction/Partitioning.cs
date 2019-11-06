namespace DragonSpark.Model.Sequences.Query.Construction
{
	public readonly struct Partitioning
	{
		public Partitioning(Selection selection, Assigned<uint> limit)
		{
			Selection = selection;
			Limit     = limit;
		}

		public Selection Selection { get; }

		public Assigned<uint> Limit { get; }
	}
}
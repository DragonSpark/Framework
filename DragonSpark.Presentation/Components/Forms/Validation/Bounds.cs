namespace DragonSpark.Presentation.Components.Forms.Validation
{
	public readonly struct Bounds
	{
		public Bounds(uint minimum, uint maximum)
		{
			Minimum = minimum;
			Maximum = maximum;
		}

		public uint Minimum { get; }

		public uint Maximum { get; }
	}
}
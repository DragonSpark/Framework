namespace DragonSpark.Model.Selection
{
	public readonly struct Decoration<TIn, TOut>
	{
		public Decoration(TIn parameter, TOut result)
		{
			Parameter = parameter;
			Result    = result;
		}

		public TIn Parameter { get; }

		public TOut Result { get; }
	}
}
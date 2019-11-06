namespace DragonSpark.Text
{
	sealed class None : Text
	{
		public static None Default { get; } = new None();

		None() : base("N/A") {}
	}
}
namespace DragonSpark.Model.Sequences.Memory
{
	public static class LeaseExtensions
	{
		public static Lease<T> Distinct<T>(this in Lease<T> @this) => Memory.Distinct<T>.Default.Get(in @this);
	}
}
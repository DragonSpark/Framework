namespace DragonSpark.Model.Sequences
{
	public sealed class Leases<T> : Storage<T>
	{
		public static Leases<T> Default { get; } = new Leases<T>();

		Leases() : base(Allotted<T>.Default, Return<T>.Default) {}
	}
}
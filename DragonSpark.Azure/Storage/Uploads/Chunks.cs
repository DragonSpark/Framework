namespace DragonSpark.Azure.Storage.Uploads;

sealed class Chunks : FormValue<ushort>
{
	public static Chunks Default { get; } = new();

	Chunks() : base("total-chunk", ushort.Parse) {}
}
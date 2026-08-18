namespace DragonSpark.Azure.Storage.Uploads;

sealed class Chunk : FormValue<ushort>
{
	public static Chunk Default { get; } = new();

	Chunk() : base("chunk-index", ushort.Parse) {}
}
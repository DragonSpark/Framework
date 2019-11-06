namespace DragonSpark.Model.Sequences.Query
{
	public delegate void Copy<in TIn, in TOut>(TIn[] source, TOut[] destination, uint from, uint to, uint offset);
}
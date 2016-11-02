namespace DragonSpark.Sources.Parameterized
{
	public interface ICurry<in T1, out TResult> : ICurry<T1, object, TResult> {}
}
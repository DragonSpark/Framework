namespace DragonSpark.Model.Results
{
	public interface IResult<out T>
	{
		T Get();
	}

	public static class Extensions
	{
		public static IResult<T> AsDefined<T>(this IResult<T> @this) => @this;
	}
}
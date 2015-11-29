using DragonSpark.Runtime;

namespace DragonSpark.Extensions
{
	public static class FactoryExtensions
	{
		public static TResult Create<TResult>( this IFactory target, object source = null )
		{
			var result = target.Create( typeof(TResult), source ).To<TResult>();
			return result;
		}
	}
}
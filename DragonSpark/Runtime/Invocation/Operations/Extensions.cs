using System.Threading.Tasks;

namespace DragonSpark.Runtime.Invocation.Operations
{
	public static class Extensions
	{
		public static T Wait<T>(this Task<T> @this)
		{
			@this.Wait();
			return @this.Result;
		}
	}
}
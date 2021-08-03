using DragonSpark.Compose.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Server.Communication
{
	public static class ExtensionMethods
	{
		public static Selector<TIn, TOut> Request<TIn, T, TOut>(this Selector<TIn, T> @this,
		                                                        Func<T, Task<TOut>> parameter)
			=> @this.Select(new Request<T, TOut>(parameter))
			        .Select(Request<TOut>.Default);
	}
}
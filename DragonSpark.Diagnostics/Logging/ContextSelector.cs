using Serilog;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Diagnostics.Logging
{
	public sealed class ContextSelector<T> : IAlteration<ILogger>
	{
		public static ContextSelector<T> Default { get; } = new ContextSelector<T>();

		ContextSelector() {}

		public ILogger Get(ILogger parameter) => parameter.ForContext<T>();
	}
}
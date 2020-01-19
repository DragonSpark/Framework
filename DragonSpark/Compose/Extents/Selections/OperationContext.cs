using DragonSpark.Compose.Model;
using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Compose.Extents.Selections
{
	public sealed class OperationContext<T> : Selector<T, ValueTask>
	{
		readonly ISelect<T, ValueTask> _subject;

		public OperationContext(ISelect<T, ValueTask> subject) : base(subject) => _subject = subject;

		public LogOperationContext<T, TParameter> Bind<TParameter>(ILogMessage<TParameter> log)
			=> new LogOperationContext<T, TParameter>(_subject, log);

		public LogOperationExceptionContext<T, TParameter> Bind<TParameter>(ILogException<TParameter> log)
			=> new LogOperationExceptionContext<T, TParameter>(_subject, log);
	}
}

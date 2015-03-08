using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Commands
{
	public abstract class MonitoredCommandBase<TContext> : MonitoredCommandBase
	{
		public TContext ContextChecked
		{
			get { return Context.To<TContext>(); }
		}

		protected override System.Type ContextType
		{
			get { return typeof(TContext); }
		}
	}
}
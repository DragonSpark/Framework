using Serilog;
using DragonSpark.Model.Commands;
using DragonSpark.Runtime.Activation;

namespace DragonSpark.Diagnostics.Logging
{
	public interface ILogMessage<in T> : ICommand<T>, IActivateUsing<ILogger> {}
}
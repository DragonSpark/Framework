using DragonSpark.Model.Commands;
using DragonSpark.Runtime.Activation;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Diagnostics.Logging
{
	public interface ILogMessage<in T> : ICommand<T>, IActivateUsing<ILogger> {}
}
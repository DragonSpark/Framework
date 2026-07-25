using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.Runtime.Operations.Execution;

public interface IOperations : ICommand<Func<ValueTask>>, IStopAware;
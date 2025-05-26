using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations.Stop;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime.Operations.Execution;

public interface IOperations : ICommand<Func<ValueTask>>, IStopAware;
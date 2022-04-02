using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime.Operations.Execution;

public interface IOperations : ICommand<Func<ValueTask>>, IOperation {}
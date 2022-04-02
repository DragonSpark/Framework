using DragonSpark.Model.Commands;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime.Operations.Execution;

public interface IAmbientOperations : ICommand<Func<ValueTask>> {}
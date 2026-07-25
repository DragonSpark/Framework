namespace DragonSpark.Application.Runtime.Operations.Execution;

sealed class DeferredOperationsQueue : Queue<Func<ValueTask>>;
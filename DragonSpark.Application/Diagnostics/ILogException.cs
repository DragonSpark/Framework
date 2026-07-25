using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Application.Diagnostics;

public interface ILogException : ISelecting<LogExceptionInput, Exception>;
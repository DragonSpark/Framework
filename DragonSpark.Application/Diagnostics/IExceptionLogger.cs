using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Application.Diagnostics;

public interface IExceptionLogger : ISelecting<ExceptionInput, Exception>;
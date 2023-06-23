using DragonSpark.Model.Operations.Selection;
using System;

namespace DragonSpark.Application.Diagnostics;

public interface IExceptionLogger : ISelecting<ExceptionInput, Exception> {}
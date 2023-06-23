using DragonSpark.Model.Operations.Selection;
using System;

namespace DragonSpark.Application.Diagnostics;

public interface ILogException : ISelecting<LogExceptionInput, Exception> {}
using DragonSpark.Model.Operations;
using System;

namespace DragonSpark.Application.Diagnostics;

public interface ILogException : ISelecting<LogExceptionInput, Exception> {}
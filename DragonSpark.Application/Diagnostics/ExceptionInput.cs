using System;
using Exception = System.Exception;

namespace DragonSpark.Application.Diagnostics;

public readonly record struct ExceptionInput(Type Owner, Exception Exception);
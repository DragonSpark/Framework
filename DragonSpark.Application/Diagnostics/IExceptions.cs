using DragonSpark.Model.Operations;
using System;

namespace DragonSpark.Application.Diagnostics;

public interface IExceptions : IOperation<(Type Owner, Exception Exception)> {}
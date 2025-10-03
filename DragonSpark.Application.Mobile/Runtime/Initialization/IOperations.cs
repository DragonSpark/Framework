using System.Collections.Generic;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Runtime.Initialization;

public interface IOperations<T> : IMutable<List<T>?>, IOperation<None>;
using DragonSpark.Model.Operations.Selection;
using System.Threading;

namespace DragonSpark.Model.Operations.Results;

public interface IToken<T> : ISelecting<CancellationToken, T>;
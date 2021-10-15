using DragonSpark.Model.Results;
using System.Threading;

namespace DragonSpark.Model.Operations;

public interface IToken : IResult<CancellationToken> {}
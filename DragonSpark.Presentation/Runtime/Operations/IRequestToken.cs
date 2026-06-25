using System.Threading;
using DragonSpark.Model.Results;

namespace DragonSpark.Presentation.Runtime.Operations;

public interface IRequestToken : IResult<CancellationToken>;
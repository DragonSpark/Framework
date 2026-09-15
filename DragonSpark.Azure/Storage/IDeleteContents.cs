using DragonSpark.Model.Operations.Selection.Stop;
using System.Collections.Immutable;

namespace DragonSpark.Azure.Storage;

public interface IDeleteContents : IStopAware<string, ImmutableArray<DeleteContentResult>>;
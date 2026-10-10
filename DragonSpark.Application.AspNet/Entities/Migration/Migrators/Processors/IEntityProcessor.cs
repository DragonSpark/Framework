using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Source;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Processors;

public interface IEntityProcessor<T> : IStopAware<SourceInput<T>, uint>, IResult<uint?>;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Save;

public interface ISave : IStopAware<SaveInput, uint>, IResult<uint?>;
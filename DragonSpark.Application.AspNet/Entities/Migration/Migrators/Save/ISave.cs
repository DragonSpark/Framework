using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Save;

public interface ISave : IStopAware<SaveInput, uint>;
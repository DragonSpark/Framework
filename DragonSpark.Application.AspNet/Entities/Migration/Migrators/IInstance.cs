using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public interface IInstance : IStopAware<MappingInput, object>;
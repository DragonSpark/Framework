using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public interface ISave<T> : IStopAware<SaveInput<T>, uint> where T : class;
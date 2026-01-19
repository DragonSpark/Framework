using DragonSpark.Model.Selection;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public interface ISaveBatch<T> : ISelect<SaveBatchInput<T>, uint> where T : class;
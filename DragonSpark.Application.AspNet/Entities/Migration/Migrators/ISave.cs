using DragonSpark.Model.Selection;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public interface ISave<T> : ISelect<SaveInput<T>, uint> where T : class;
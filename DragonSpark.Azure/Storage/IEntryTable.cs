using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Azure.Storage;

public interface IEntryTable : ISelect<IDictionary<string, string?>, ITable<string, string?>>;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using System.Collections.Generic;

namespace DragonSpark.Azure.Storage;

public interface IEntryTable : ISelect<IDictionary<string, string?>, ITable<string, string?>>;
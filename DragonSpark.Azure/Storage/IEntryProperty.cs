using DragonSpark.Model;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Properties;
using DragonSpark.Model.Selection;
using System.Collections.Generic;

namespace DragonSpark.Azure.Storage;

public interface IEntryProperty : IProperty<IDictionary<string, string?>, string?>, ISelect<IStorageEntry, string?>, 
                                  IStopAware<Pair<IStorageEntry, string>>;
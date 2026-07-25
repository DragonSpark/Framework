using DragonSpark.Model;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Properties;
using DragonSpark.Model.Selection;

namespace DragonSpark.Azure.Storage;

public interface IEntryProperty : IProperty<IDictionary<string, string?>, string?>, ISelect<IStorageEntry, string?>, 
                                  IStopAware<Pair<IStorageEntry, string>>;
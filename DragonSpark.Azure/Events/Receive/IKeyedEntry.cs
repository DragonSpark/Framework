using DragonSpark.Model.Selection;

namespace DragonSpark.Azure.Events.Receive;

public interface IKeyedEntry : ISelect<EntryKey, RegistryEntry>;
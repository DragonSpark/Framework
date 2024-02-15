using DragonSpark.Model.Selection;

namespace DragonSpark.Azure.Messaging.Events.Receive;

public interface IKeyedEntry : ISelect<EntryKey, RegistryEntry>;
using DragonSpark.Model.Selection;

namespace DragonSpark.Azure.Messaging.Messages.Topics.Receive;

public interface IKeyedEntry : ISelect<EntryKey, RegistryEntry>;
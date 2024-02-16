using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Azure.Messaging.Messages.Topics.Receive;

public interface IEntries : ITable<EntryKey, RegistryEntry>, ICommand;
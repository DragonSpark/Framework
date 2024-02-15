using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Azure.Messaging.Events.Receive;

public interface IEventRegistration : ICommand<ITable<EntryKey, RegistryEntry>>;
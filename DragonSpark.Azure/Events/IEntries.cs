using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Azure.Events;

public interface IEntries : ITable<EntryKey, RegistryEntry>, ICommand;
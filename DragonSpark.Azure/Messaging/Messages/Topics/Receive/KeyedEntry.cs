using DragonSpark.Compose;
using DragonSpark.Model.Selection.Stores;
using System;

namespace DragonSpark.Azure.Messaging.Messages.Topics.Receive;

public sealed class KeyedEntry<T> : KeyedEntry
{
	public static KeyedEntry<T> Default { get; } = new();

	KeyedEntry() : base(_ => new(A.Type<T>())) {}
}

public class KeyedEntry : IKeyedEntry
{
	readonly ITable<EntryKey, RegistryEntry> _registry;
	readonly Func<EntryKey, RegistryEntry>   _new;

	protected KeyedEntry(Func<EntryKey, RegistryEntry> @new) : this(Entries.Default, @new) {}

	protected KeyedEntry(ITable<EntryKey, RegistryEntry> registry, Func<EntryKey, RegistryEntry> @new)
	{
		_registry = registry;
		_new      = @new;
	}

	public RegistryEntry Get(EntryKey parameter)
		=> _registry.TryGet(parameter, out var current)
			   ? current
			   : _registry.Parameter(new(parameter, _new(parameter))).Value;
}
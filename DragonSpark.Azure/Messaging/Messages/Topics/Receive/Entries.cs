﻿using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Azure.Messaging.Messages.Topics.Receive;

sealed class Entries : ConcurrentTable<EntryKey, RegistryEntry>, IEntries
{
	public static Entries Default { get; } = new();

	Entries() {}
}
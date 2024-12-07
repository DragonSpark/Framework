namespace DragonSpark.Application.Compose.Store.Operations.Distributed;

readonly record struct EntryKey<T>(T Parameter, string Key);
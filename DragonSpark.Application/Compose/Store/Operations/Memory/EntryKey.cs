namespace DragonSpark.Application.Compose.Store.Operations.Memory;

readonly record struct EntryKey<T>(T Parameter, object Key);
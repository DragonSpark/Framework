namespace DragonSpark.Application.Compose.Store.Operations;

readonly record struct EntryKey<T>(T Parameter, object Key);
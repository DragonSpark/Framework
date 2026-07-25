using DragonSpark.Model;

namespace DragonSpark.Text.Formatting;

public interface IFormatEntry<T> : IPair<string, Func<T, string>>;
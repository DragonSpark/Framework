using DragonSpark.Model.Selection;

namespace DragonSpark.Text.Formatting;

public interface ISelectFormatter<in T> : ISelect<string?, T, string> {}
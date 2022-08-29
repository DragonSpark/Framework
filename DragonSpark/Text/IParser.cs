using DragonSpark.Model.Selection;

namespace DragonSpark.Text;

public interface IParser<out T> : ISelect<string, T> {}
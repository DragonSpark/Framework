using DragonSpark.Model.Selection;

namespace DragonSpark.Text;

public interface IFormatter<in T> : ISelect<T, string> {}
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Runtime.Objects;

public interface IMaterialize<out T> : ISelect<Array<byte>, T>;
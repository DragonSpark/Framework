using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Properties;

public interface IProperty<THost, TValue> : ISelect<THost, TValue>, IAssign<THost, TValue>;
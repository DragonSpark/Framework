using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Model.Properties;

public interface IProperty<THost, TValue> : IConditional<THost, TValue>, IAssign<THost, TValue> {}
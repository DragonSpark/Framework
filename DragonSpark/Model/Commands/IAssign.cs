namespace DragonSpark.Model.Commands;

public interface IAssign<TKey, TValue> : ICommand<Pair<TKey, TValue>> {}
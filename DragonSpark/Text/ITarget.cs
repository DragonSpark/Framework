using DragonSpark.Model.Commands;

namespace DragonSpark.Text;

public interface ITarget<T> : ICommand<TargetInput<T>>;
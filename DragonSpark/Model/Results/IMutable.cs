using DragonSpark.Model.Commands;

namespace DragonSpark.Model.Results;

public interface IMutable<T> : IResult<T>, ICommand<T> {}

// TODO:

public interface ISwitch : IMutable<bool> {}

public sealed class Switch : Variable<bool>, ISwitch
{
	public Switch(bool instance = default) : base(instance) {}
}
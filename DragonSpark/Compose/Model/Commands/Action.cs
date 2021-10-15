using DragonSpark.Model;
using DragonSpark.Model.Commands;

namespace DragonSpark.Compose.Model.Commands;

public class Action<T> : Command<T>, IAction<T>
{
	public static implicit operator Action<T>(System.Action<T> value) => new Action<T>(value);

	public Action(System.Action<T> body) : base(body) {}

	public None Get(T parameter)
	{
		Execute(parameter);
		return None.Default;
	}
}

public class Action : Command, IAction<None>
{
	public static implicit operator Action(System.Action value) => new Action(value);

	public Action(System.Action action) : base(action) {}

	public None Get(None parameter)
	{
		Execute(parameter);
		return None.Default;
	}
}
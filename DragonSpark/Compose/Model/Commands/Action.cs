using DragonSpark.Model;
using DragonSpark.Model.Commands;

namespace DragonSpark.Compose.Model.Commands;

public class Action<T> : Command<T>, IAction<T>
{
	public static implicit operator Action<T>(System.Action<T> value) => new(value);

	public Action(System.Action<T> body) : base(body) {}

	public None Get(T parameter)
	{
		Execute(parameter);
		return None.Default;
	}
}
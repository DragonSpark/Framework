using DragonSpark.Model.Commands;
using DragonSpark.Runtime;

namespace DragonSpark.Model.Selection.Adapters
{
	public class Action<T> : Command<T>, IAction<T>
	{
		public Action(System.Action<T> body) : base(body) {}

		public None Get(T parameter)
		{
			Execute(parameter);
			return None.Default;
		}

		public static implicit operator Action<T>(System.Action<T> value) => new Action<T>(value);
	}

	public class Action : Command, IAction<None>, ICommand
	{
		public Action(System.Action action) : base(action) {}

		public None Get(None parameter)
		{
			Execute(parameter);
			return None.Default;
		}

		public static implicit operator Action(System.Action value) => new Action(value);
	}
}
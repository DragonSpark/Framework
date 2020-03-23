using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Sequences;
using Array = System.Array;

namespace DragonSpark.Runtime.Environment
{
	public sealed class Add<T> : ICommand<T>
	{
		readonly IMutable<Array<T>> _array;

		public Add(IMutable<Array<T>> array) => _array = array;

		public void Execute(T parameter)
		{
			var current = _array.Get().Open();
			var to      = current.Length;
			Array.Resize(ref current, to + 1);
			current[to] = parameter;
			_array.Execute(current);
		}
	}
}
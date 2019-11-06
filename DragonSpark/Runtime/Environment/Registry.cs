using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Runtime.Environment
{
	public class Registry<T> : ArrayResult<T>, IRegistry<T>
	{
		readonly ICommand<T>                        _add;
		readonly ICommand<Model.Sequences.Store<T>> _range;

		public Registry() : this(Empty<T>.Array) {}

		public Registry(params T[] elements) : this(new Array<T>(elements)) {}

		public Registry(Array<T> elements) : this(new Variable<Array<T>>(elements)) {}

		public Registry(IMutable<Array<T>> source) : this(source, new AddRange<T>(source), new Add<T>(source)) {}

		public Registry(IResult<Array<T>> source, ICommand<Model.Sequences.Store<T>> range, ICommand<T> add)
			: base(source)
		{
			_range = range;
			_add   = add;
		}

		public void Execute(T parameter)
		{
			_add.Execute(parameter);
		}

		public void Execute(Model.Sequences.Store<T> parameter)
		{
			_range.Execute(parameter);
		}
	}
}
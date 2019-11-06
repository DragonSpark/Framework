using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Sequences
{
	sealed class Arrays<T> : ISelect<Store<T>, T[]>
	{
		public static Arrays<T> Default { get; } = new Arrays<T>();

		Arrays() : this(Selection.Default) {}

		readonly Assigned<uint> _length;

		readonly uint _start;

		public Arrays(Selection selection) : this(selection.Start, selection.Length) {}

		public Arrays(uint start, Assigned<uint> length)
		{
			_start  = start;
			_length = length;
		}

		public T[] Get(Store<T> parameter)
		{
			var size = _length.IsAssigned
				           ? _length.Instance
				           : parameter.Length - _start;
			var result = parameter.Instance.CopyInto(new T[size], _start, size);
			return result;
		}
	}
}
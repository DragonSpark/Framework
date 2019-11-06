using System;
using DragonSpark.Model.Results;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Runtime.Environment
{
	public sealed class AddRange<T> : IAddRange<T>
	{
		readonly IMutable<Array<T>> _array;

		public AddRange(IMutable<Array<T>> array) => _array = array;

		public void Execute(Model.Sequences.Store<T> parameter)
		{
			var current = _array.Get().Open();
			var length  = parameter.Length;
			var to      = (uint)current.Length;
			Array.Resize(ref current, (int)(length + to));
			parameter.Instance.CopyInto(current, 0, length, to);
			_array.Execute(current);
		}
	}
}
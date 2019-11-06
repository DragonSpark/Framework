using DragonSpark.Model.Sequences;
using System;
using System.Linq;

namespace DragonSpark.Testing.Objects
{
	sealed class NativeArray : IArray<int>
	{
		public static NativeArray Default { get; } = new NativeArray();

		NativeArray() : this(Select.Default, Data.Default) {}

		readonly string[] _data;

		readonly Func<string, int> _select;

		public NativeArray(Func<string, int> select, string[] data)
		{
			_select = select;
			_data   = data;
		}

		public Array<int> Get() => _data.Select(_select).Where(x => x > 0).ToArray();
	}
}
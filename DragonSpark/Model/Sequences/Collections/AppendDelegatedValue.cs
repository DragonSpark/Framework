using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using System;

namespace DragonSpark.Model.Sequences.Collections
{
	public sealed class AppendDelegatedValue<T> : IAlteration<Array<T>>
	{
		readonly Func<T> _item;

		public AppendDelegatedValue(Func<T> item) => _item = item;

		public Array<T> Get(Array<T> parameter) => parameter.Open().Append(_item()).Result();
	}
}
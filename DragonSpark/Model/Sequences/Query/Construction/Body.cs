using System;

namespace DragonSpark.Model.Sequences.Query.Construction
{
	sealed class Body<T> : IBody<T>
	{
		readonly Selection _selection;

		public Body(Selection selection) => _selection = selection;

		public ArrayView<T> Get(ArrayView<T> parameter)
			=> new ArrayView<T>(parameter.Array, _selection.Start,
			                    _selection.Length.IsAssigned
				                    ? Math.Min(parameter.Length - _selection.Start, _selection.Length)
				                    : parameter.Length - _selection.Start);
	}
}
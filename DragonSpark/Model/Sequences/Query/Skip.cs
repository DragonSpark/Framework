using System;
using DragonSpark.Model.Sequences.Query.Construction;

namespace DragonSpark.Model.Sequences.Query
{
	sealed class Skip : IPartition
	{
		readonly uint _count;

		public Skip(uint count) => _count = count;

		public Selection Get(Selection parameter)
		{
			var count = parameter.Start + _count;
			var result = new Selection(parameter.Length.IsAssigned ? Math.Min(parameter.Length, count) : count,
			                           parameter.Length);
			return result;
		}
	}
}
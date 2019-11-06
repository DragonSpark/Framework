using System;
using DragonSpark.Model.Sequences.Query.Construction;

namespace DragonSpark.Model.Sequences.Query
{
	sealed class Take : IPartition
	{
		readonly uint _count;

		public Take(uint count) => _count = count;

		public Selection Get(Selection parameter)
			=> new Selection(parameter.Start,
			                 parameter.Length.IsAssigned ? Math.Min(parameter.Length, _count) : _count);
	}
}
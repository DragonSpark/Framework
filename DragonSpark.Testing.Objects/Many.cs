using AutoFixture;
using DragonSpark.Model.Selection;
using System.Collections.Generic;

namespace DragonSpark.Testing.Objects
{
	sealed class Many<T> : ISelect<IFixture, IEnumerable<T>>
	{
		readonly int _count;

		public Many(uint count) => _count = (int)count;

		public IEnumerable<T> Get(IFixture parameter) => parameter.CreateMany<T>(_count);
	}
}
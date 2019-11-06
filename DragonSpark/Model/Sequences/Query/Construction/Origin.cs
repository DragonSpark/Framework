using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Sequences.Query.Construction
{
	sealed class Origin<_, T> : ISelect<_, Store<T>>
	{
		readonly IEnter<T>       _enter;
		readonly ISelect<_, T[]> _origin;

		public Origin(ISelect<_, T[]> origin) : this(origin, Enter<T>.Default) {}

		public Origin(ISelect<_, T[]> origin, IEnter<T> enter)
		{
			_origin = origin;
			_enter  = enter;
		}

		public Store<T> Get(_ parameter) => _enter.Get(_origin.Get(parameter));
	}
}
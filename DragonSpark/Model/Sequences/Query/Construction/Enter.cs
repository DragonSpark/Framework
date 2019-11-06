using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Sequences.Query.Construction
{
	sealed class Enter<T> : Model.Selection.Select<T[], Store<T>>, IEnter<T>
	{
		public static Enter<T> Default { get; } = new Enter<T>();

		Enter() : base(x => new Store<T>(x)) {}
	}

	sealed class Enter<_, T> : ISelect<_, Store<T>>
	{
		readonly IEnter<T>       _enter;
		readonly ISelect<_, T[]> _origin;

		public Enter(ISelect<_, T[]> origin) : this(origin, Enter<T>.Default) {}

		public Enter(ISelect<_, T[]> origin, IEnter<T> enter)
		{
			_origin = origin;
			_enter  = enter;
		}

		public Store<T> Get(_ parameter) => _enter.Get(_origin.Get(parameter));
	}
}
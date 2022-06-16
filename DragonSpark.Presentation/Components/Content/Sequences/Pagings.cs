namespace DragonSpark.Presentation.Components.Content.Sequences;

/*sealed class Pagings<T> : Validating<PagingInput<T>, IPaging<T>>
{
	public Pagings(IDepending<IQueries<T>> any, IPagers<T> previous)
		: this(Start.A.Selection<PagingInput<T>>().By.Calling(x => x.Queries).Select(any).Then(), previous,
		       EmptyPaging<T>.Default) {}

	public Pagings(Await<PagingInput<T>, bool> any, IPagers<T> previous, IPaging<T> @default)
		: base(any, previous.Then().Operation(), @default.Start().Accept<PagingInput<T>>().Operation()) {}
}*/
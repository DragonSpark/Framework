namespace DragonSpark.Model.Sequences.Query.Construction
{
	public delegate IContent<TIn, TOut> Create<TIn, TOut, in TParameter>(IBody<TIn> body,
	                                                                     IStores<TOut> stores, TParameter parameter,
	                                                                     Assigned<uint> limit);

	public delegate IBody<T> Create<T, in TParameter>(TParameter parameter, Selection selection, Assigned<uint> limit);
}
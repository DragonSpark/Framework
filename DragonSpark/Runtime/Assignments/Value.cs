namespace DragonSpark.Runtime.Assignments
{
	public struct Value<T>
	{
		public Value( T start, T finish = default(T) )
		{
			Start = start;
			Finish = finish;
		}

		public T Start { get; }
		public T Finish { get; }
	}
}
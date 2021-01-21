using DragonSpark.Model.Sequences;

namespace DragonSpark.Syncfusion.Queries
{
	sealed class DefaultQuery<T> : ArrayInstance<IQuery<T>>
	{
		public static DefaultQuery<T> Default { get; } = new DefaultQuery<T>();

		DefaultQuery() : base(Search<T>.Default, Sort<T>.Default, Where<T>.Default, Count<T>.Default, Skip<T>.Default,
		                      Take<T>.Default) {}
	}
}
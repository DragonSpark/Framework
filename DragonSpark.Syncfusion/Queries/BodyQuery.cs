using DragonSpark.Model.Operations;

namespace DragonSpark.Syncfusion.Queries
{
	sealed class BodyQuery<T> : Alterings<Parameter<T>>, IQuery<T>
	{
		public static BodyQuery<T> Default { get; } = new BodyQuery<T>();

		BodyQuery() : base(Search<T>.Default, Where<T>.Default, Sort<T>.Default) {}
	}
}
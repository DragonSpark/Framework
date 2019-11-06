using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Adapters;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Testing.Objects
{
	public sealed class Sequencing<T>
	{
		public static Sequencing<T> Default { get; } = new Sequencing<T>();

		Sequencing() : this(Start.A.Selection<T>().As.Sequence.Array.By.Self.Query()) {}

		public Sequencing(Query<T[], T> sequence) : this(sequence, Objects.Near.Default, Objects.Far.Default) {}

		public Sequencing(Query<T[], T> sequence, Selection near, Selection far)
			: this(sequence.Out(), sequence.Select(near).Out(), sequence.Select(far).Out()) {}

		public Sequencing(ISelect<T[], T[]> full, ISelect<T[], T[]> near, ISelect<T[], T[]> far)
		{
			Full = full;
			Near = near;
			Far  = far;
		}

		public ISelect<T[], T[]> Full { get; }
		public ISelect<T[], T[]> Near { get; }
		public ISelect<T[], T[]> Far { get; }

		public Sequencing<T> Get(ISelect<T[], T[]> select)
			=> new Sequencing<T>(Full.Select(select), Near.Select(select), Far.Select(select));
	}
}
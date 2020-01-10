using DragonSpark.Compose;
using DragonSpark.Compose.Model;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
// ReSharper disable UnusedParameter.Local

namespace DragonSpark.Testing.Objects
{
	public sealed class Sequencing<T>
	{
		public static Sequencing<T> Default { get; } = new Sequencing<T>();

		Sequencing() : this(Start.A.Selection<T>().As.Sequence.Open.By.Self.Query()) {}

		public Sequencing(OpenQuerySelector<T[], T> sequence) : this(sequence, Objects.Near.Default, Objects.Far.Default) {}

		public Sequencing(OpenQuerySelector<T[], T> sequence, Selection near, Selection far)
			: this(sequence.Out(), null, null) {} // TODO: assign.

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
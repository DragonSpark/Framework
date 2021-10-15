using DragonSpark.Model.Operations;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Runtime.Materialize;

public sealed class Counting<T> : Selecting<IQueryable<T>, uint>, ICount<T>
{
	public static Counting<T> Default { get; } = new Counting<T>();

	Counting() : this(DefaultCount<T>.Default, DefaultLargeCount<T>.Default) {}

	public Counting(ICount<T> count, ILargeCount<T> large) : base(count) => Large = large;

	public ILargeCount<T> Large { get; }
}
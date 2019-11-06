using System;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Model.Sequences.Query
{
	public sealed class Only<T> : One<T>
	{
		public static Only<T> Default { get; } = new Only<T>();

		Only() : this(Always<T>.Default.Get) {}

		public Only(Func<T, bool> where) : base(where) {}
	}
}
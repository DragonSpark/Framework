using System;
using DragonSpark.Model.Selection;

namespace DragonSpark.Runtime.Objects
{
	sealed class Projectors : Select<Type, string, Func<object, IProjection>>, IProjectors
	{
		public static Projectors Default { get; } = new Projectors();

		Projectors() : base(KnownProjectors.Default.Select(x => x.Open().ToStore().ToDelegate()).Assume()) {}
	}
}
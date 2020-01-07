using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Runtime.Objects
{
	sealed class Projectors : Select<Type, string, Func<object, IProjection>>, IProjectors
	{
		public static Projectors Default { get; } = new Projectors();

		Projectors() : base(KnownProjectors.Default.Then()
		                                   .Select(x => x.Open().ToStore().ToDelegate())
		                                   .Get()
		                                   .Then()
		                                   .Assume()) {}
	}
}
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Sequences;
using System;

namespace DragonSpark.Runtime.Objects
{
	sealed class KnownProjectors : ArrayInstance<Pair<Type, Func<string, Func<object, IProjection>>>>
	{
		public static KnownProjectors Default { get; } = new KnownProjectors();

		KnownProjectors() : base(ApplicationDomainProjection.Default.Entry()) {}
	}
}
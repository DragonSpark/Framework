using System;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Runtime.Objects
{
	sealed class KnownProjectors : ArrayInstance<Pair<Type, Func<string, Func<object, IProjection>>>>
	{
		public static KnownProjectors Default { get; } = new KnownProjectors();

		KnownProjectors() : base(ApplicationDomainProjection.Default.Entry()) {}
	}
}
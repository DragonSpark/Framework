using DragonSpark.Sources;
using System;
using System.Composition;

namespace DragonSpark.Testing.Objects
{
	[Export]
	public class FactoryOfYac : SourceBase<YetAnotherClass>
	{
		readonly Func<YetAnotherClass> inner;

		public FactoryOfYac() : this( () => new YetAnotherClass() ) {}

		FactoryOfYac( Func<YetAnotherClass> inner )
		{
			this.inner = inner;
		}

		public override YetAnotherClass Get() => inner();
	}
}
using System;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Testing.Objects
{
	[Discoverable]
	public class FactoryOfYAC : FactoryBase<YetAnotherClass>
	{
		readonly Func<YetAnotherClass> inner;

		public FactoryOfYAC() : this( ActivateFactory<YetAnotherClass>.Instance.Create ) {}

		FactoryOfYAC( [Required] Func<YetAnotherClass> inner )
		{
			this.inner = inner;
		}

		protected override YetAnotherClass CreateItem() => inner();
	}
}
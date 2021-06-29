using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using LightInject;
using System;
using System.Reflection;
using Array = DragonSpark.Model.Sequences.Array;

namespace DragonSpark.Composition.Construction
{
	sealed class CanSelectDependency : AllCondition<ParameterInfo>
	{
		public CanSelectDependency(ServiceContainer owner, ContainerOptions options)
			: this(new CanLocateDependency(owner.CanGetInstance, options.EnableOptionalArguments),
			       new IsValidDependency(owner, Array.Of(typeof(Func<,>), typeof(Func<,,>)))) {}

		public CanSelectDependency(CanLocateDependency locate, IsValidDependency valid)
			: base(locate.Then(),
			       Start.A.Selection<ParameterInfo>().By.Calling(x => x.ParameterType).Select(valid)) {}
	}
}
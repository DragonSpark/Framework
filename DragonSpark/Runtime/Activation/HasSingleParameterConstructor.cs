using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using DragonSpark.Reflection.Members;
using DragonSpark.Reflection.Types;
using System.Reflection;

namespace DragonSpark.Runtime.Activation
{
	sealed class HasSingleParameterConstructor<T> : Condition<ConstructorInfo>
	{
		public static HasSingleParameterConstructor<T> Default { get; } = new HasSingleParameterConstructor<T>();

		HasSingleParameterConstructor() : this(Parameters.Default) {}

		public HasSingleParameterConstructor(ISelect<ConstructorInfo, Array<ParameterInfo>> parameters)
			: base(parameters.Query()
			                 .FirstAssigned()
			                 .Select(Start.An.Instance(ParameterType.Default)
			                              .Then()
			                              .Metadata()
			                              .Select(IsAssignableFrom<T>.Default)
			                              .Assigned()
			                              .Get())
			                 .Then()
			                 .And(parameters.Then().Select(RemainingParametersAreOptional.Default))) {}
	}
}
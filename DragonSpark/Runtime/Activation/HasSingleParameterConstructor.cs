using DragonSpark.Compose;
using DragonSpark.Compose.Model;
using DragonSpark.Reflection.Members;
using DragonSpark.Reflection.Types;
using System.Reflection;

namespace DragonSpark.Runtime.Activation
{
	sealed class HasSingleParameterConstructor<T> : Model.Selection.Conditions.Condition<ConstructorInfo>
	{
		public static HasSingleParameterConstructor<T> Default { get; } = new HasSingleParameterConstructor<T>();

		HasSingleParameterConstructor() : this(Parameters.Default.Then()) {}

		public HasSingleParameterConstructor(ArraySelector<ConstructorInfo, ParameterInfo> parameters)
			: base(parameters.FirstAssigned()
			                 .Select(Start.An.Instance(ParameterType.Default)
			                              .Then()
			                              .Metadata()
			                              .Select(IsAssignableFrom<T>.Default)
			                              .EnsureAssignedOrDefault()
			                              .Get())
			                 .Get() // TODO: Get/Then
			                 .Then()
			                 .And(parameters.Subject.Select(RemainingParametersAreOptional.Default))) {}
	}
}
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Reflection.Types;
using System;

namespace DragonSpark.Runtime.Activation
{
	sealed class CanActivate : Condition<Type>
	{
		public static CanActivate Default { get; } = new CanActivate();

		CanActivate() : base(HasSingletonProperty.Default.Then()
		                                         .Or(IsClass.Default.Then()
		                                                    .And(HasActivationConstructor.Default))) {}
	}
}
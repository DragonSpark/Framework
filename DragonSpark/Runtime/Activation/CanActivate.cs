using System;
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Runtime.Activation;

sealed class CanActivate : Condition<Type>
{
    public static CanActivate Default { get; } = new();

    CanActivate() : base(HasSingletonProperty.Default.Then()
                                             .Or(CanConstruct.Default.Then()
                                                             .And(HasActivationConstructor.Default))) {}
}
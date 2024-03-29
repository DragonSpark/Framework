﻿using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Conditions;

namespace DragonSpark.Application.Model.Interaction;

sealed class ValidatingAwareResultHandler<T> : Validating<IInteractionResult>, IInteractionResultHandler
{
	public ValidatingAwareResultHandler(IOperation<IInteractionResult> @true)
		: this(Is.Of<T>().Operation().Out(), @true) {}

	public ValidatingAwareResultHandler(IDepending<IInteractionResult> condition, IOperation<IInteractionResult> @true)
		: base(condition, @true) {}
}
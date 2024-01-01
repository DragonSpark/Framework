﻿using DragonSpark.Model.Operations;

namespace DragonSpark.Azure.Events;

public class UserEventRegistration<T> : EventRegistration<T, uint> where T : NumberMessage
{
	protected UserEventRegistration(IOperation<uint> body) : base(body) {}
}
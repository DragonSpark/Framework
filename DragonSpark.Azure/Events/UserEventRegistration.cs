using DragonSpark.Application.Diagnostics;
using DragonSpark.Model.Operations;

namespace DragonSpark.Azure.Events;

public class UserEventRegistration<T> : EventRegistration<T, uint> where T : UserMessage
{
	protected UserEventRegistration(IOperation<uint> body, IExceptionLogger logger) : base(body, logger) {}
}
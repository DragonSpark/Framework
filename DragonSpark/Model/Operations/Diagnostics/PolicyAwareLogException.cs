using DragonSpark.Compose;
using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Conditions;
using Exception = System.Exception;

namespace DragonSpark.Model.Operations.Diagnostics
{
	sealed class PolicyAwareLogException<T> : ValidatedCommand<ExceptionParameter<T>>, ILogException<T>
	{
		public PolicyAwareLogException(ICondition<Exception> condition,
		                               ICommand<ExceptionParameter<T>> command)
			: this(Start.A.Selection<ExceptionParameter<T>>().By.Calling(x => x.Exception).Select(condition).Out(),
			       command) {}

		public PolicyAwareLogException(ICondition<ExceptionParameter<T>> condition,
		                               ICommand<ExceptionParameter<T>> command) : base(condition, command) {}
	}
}
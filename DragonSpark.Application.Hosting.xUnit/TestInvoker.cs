using DragonSpark.Compose;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace DragonSpark.Application.Hosting.xUnit
{
	// ReSharper disable LocalSuppression

	sealed class TestInvoker : XunitTestInvoker
	{
		// ReSharper disable once TooManyDependencies
		public TestInvoker(ITest test, IMessageBus messageBus, Type testClass, object[] constructorArguments,
		                   MethodInfo testMethod, object[] testMethodArguments,
		                   IReadOnlyList<BeforeAfterTestAttribute> beforeAfterAttributes,
		                   ExceptionAggregator aggregator,
		                   CancellationTokenSource cancellationTokenSource)
			: base(test, messageBus, testClass, constructorArguments, testMethod, testMethodArguments,
			       beforeAfterAttributes,
			       aggregator, cancellationTokenSource) {}

		protected override object CallTestMethod(object testClassInstance)
			=> Result(testClassInstance).ContinueWith(task => task.Exception?
			                                                      .InnerExceptions
			                                                      .Select(x => x.Demystify())
			                                                      .ForEach(Aggregator.Add),
			                                          TaskContinuationOptions.OnlyOnFaulted);

		Task Result(object instance)
		{
			try
			{
				return GetTaskFromResult(base.CallTestMethod(instance));
			}
			catch (Exception e)
			{
				// ReSharper disable once UnthrowableException
				// ReSharper disable once ThrowingSystemException
				throw e.Demystify();
			}
		}
	}
}
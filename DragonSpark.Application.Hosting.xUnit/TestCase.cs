using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Runtime.Execution;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace DragonSpark.Application.Hosting.xUnit
{
	sealed class TestCase : LongLivedMarshalByRefObject, IXunitTestCase
	{
		readonly Action         _action    = default!;
		readonly IXunitTestCase _case      = default!;
		readonly ICondition     _condition = default!;
		readonly ITestMethod    _method    = default!;

		public TestCase() {}

		public TestCase(IXunitTestCase @case, Action action) : this(@case, new TestMethod(@case.TestMethod),
		                                                            new First(), action) {}

		// ReSharper disable once TooManyDependencies
		public TestCase(IXunitTestCase @case, ITestMethod method, ICondition condition, Action action)
		{
			_case      = @case;
			_method    = method;
			_condition = condition;
			_action    = action;
		}

		public void Deserialize(IXunitSerializationInfo info)
		{
			_case.Deserialize(info);
		}

		public void Serialize(IXunitSerializationInfo info)
		{
			_case.Serialize(info);
		}

		public string DisplayName => _case.DisplayName;

		public string SkipReason => _case.SkipReason;

		public ISourceInformation SourceInformation
		{
			get => _case.SourceInformation;
			set => _case.SourceInformation = value;
		}

		public ITestMethod TestMethod
		{
			get
			{
				if (_condition.Get())
				{
					_action();
					return _method;
				}

				return _case.TestMethod;
			}
		}

		public object[] TestMethodArguments => _case.TestMethodArguments;

		public Dictionary<string, List<string>> Traits => _case.Traits;

		public string UniqueID => _case.UniqueID;

		// ReSharper disable once TooManyArguments
		public Task<RunSummary> RunAsync(IMessageSink diagnosticMessageSink, IMessageBus messageBus,
		                                 object[] constructorArguments,
		                                 ExceptionAggregator aggregator,
		                                 CancellationTokenSource cancellationTokenSource)
			=> _case.RunAsync(diagnosticMessageSink, messageBus, constructorArguments, aggregator,
			                  cancellationTokenSource);

		public Exception InitializationException => _case.InitializationException;

		public IMethodInfo Method => _method.Method;
		public int Timeout => _case.Timeout;
	}
}
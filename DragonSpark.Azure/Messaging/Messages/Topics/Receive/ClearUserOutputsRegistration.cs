using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Server.Output;
using Microsoft.AspNetCore.OutputCaching;
using System;

namespace DragonSpark.Azure.Messaging.Messages.Topics.Receive;

public class ClearUserOutputsRegistration<TIn, T> : EventRegistration<T, TIn> where T : Message<TIn>
{
	protected ClearUserOutputsRegistration(Func<TIn, uint> user, IOutputCacheStore output, IUserOutputKey key)
		: this(user.Start().Operation().Out().AsStop(), output, key) {}

	protected ClearUserOutputsRegistration(IStopAware<TIn, uint> user, IOutputCacheStore output, IUserOutputKey key)
		: base(new ClearOutputs<TIn>(user, output, key)) {}
}

public class ClearUserOutputsRegistration<T> : EventRegistration<T> where T : Message
{
	protected ClearUserOutputsRegistration(Func<T, uint> user, IOutputCacheStore output, IUserOutputKey key)
		: this(user.Start().Operation().Out().AsStop(), output, key) {}

	protected ClearUserOutputsRegistration(IStopAware<T, uint> user, IOutputCacheStore output, IUserOutputKey key)
		: base(new ClearOutputs<T>(user, output, key)) {}
}
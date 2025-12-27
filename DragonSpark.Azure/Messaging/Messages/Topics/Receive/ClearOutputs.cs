using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Server.Output;
using Microsoft.AspNetCore.OutputCaching;

namespace DragonSpark.Azure.Messaging.Messages.Topics.Receive;

sealed class ClearOutputs<T> : ClearUserOutputs<T>
{
	public ClearOutputs(IStopAware<T, uint> user, IOutputCacheStore output, IUserOutputKey key) 
		: base(user, output, key) {}
}
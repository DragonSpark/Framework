using DragonSpark.Application;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace DragonSpark.Presentation.Components.Forms.Validation;

sealed class StoreAwareResourceQuery : StopAware<string, ResourceQueryRecord?>, IResourceQuery
{
	public StoreAwareResourceQuery(IResourceQuery select, IMemoryCache memory)
		: base(select.Then()
		             .Store()
		             .In(memory)
		             .For(TimeSpan.FromDays(1).Slide())
		             .Using(Start.A.Selection<Stop<string>>().By.Calling(x => x.Subject))) {}
}
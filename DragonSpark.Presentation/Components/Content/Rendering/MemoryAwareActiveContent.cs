﻿using DragonSpark.Application;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.Extensions.Caching.Memory;

namespace DragonSpark.Presentation.Components.Content.Rendering
{
	sealed class MemoryAwareActiveContent<T> : Resulting<T?>, IActiveContent<T>
	{
		public MemoryAwareActiveContent(IActiveContent<T> previous, IMemoryCache memory, string key)
			: base(previous.Then()
			               .Accept()
			               .Then()
			               .Store()
			               .In(memory)
			               .For(PreRenderingWindow.Default.Get().Slide())
			               .Using(key.Accept)
			               .Bind()) {}
	}
}
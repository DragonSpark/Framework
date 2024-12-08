﻿using System.Collections.Concurrent;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination.Memory;

public class TrackingPageKey<T> : TrackingKey<PageInput>
{
	protected TrackingPageKey(ConcurrentBag<string> track) : base(PageKey<T>.Default, track) {}
}
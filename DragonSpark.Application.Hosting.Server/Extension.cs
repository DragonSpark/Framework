﻿using DragonSpark.Application.Compose;
using DragonSpark.Composition.Compose;
using DragonSpark.Server;

namespace DragonSpark.Application.Hosting.Server
{
	public static class Extension
	{
		public static ServerProfileContext WithServerApplication(this BuildHostContext @this)
			=> @this.Apply(ServerApplicationProfile.Default);
	}
}
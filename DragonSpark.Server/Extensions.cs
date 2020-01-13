using DragonSpark.Application;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Server.Application;
using DragonSpark.Server.Compose;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Server
{
	public static class Extensions
	{
		public static ISelect<Array<string>, Task> ConfiguredBy<T>(this IProgram @this) where T : class
			=> ServerHostBuilder<T>.Default.Select(@this.Get);

		public static ServerProfileContext Apply(this BuildHostContext @this, IServerProfile profile)
			=> new ServerProfileContext(@this, profile);

		public static HostOperationsContext Operations(this ServerProfileContext @this) => @this.Get().Operations();

		public static BuildHostContext WithServer(this BuildHostContext @this, Action<IWebHostBuilder> configuration)
			=> @this.Select(new WebHostConfiguration(configuration));
	}
}
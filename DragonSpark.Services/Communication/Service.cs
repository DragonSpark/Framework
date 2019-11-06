using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Services.Communication
{
	public sealed class Service<T> : Select<Uri, T>
	{
		public static Service<T> Default { get; } = new Service<T>();

		Service() : base(ClientStore.Default.Select(Api<T>.Default)) {}
	}
}
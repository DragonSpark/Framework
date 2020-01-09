using DragonSpark.Compose;
using DragonSpark.Model.Selection.Stores;
using System;

namespace DragonSpark.Runtime
{
	public sealed class Uris : ReferenceValueStore<string, Uri>
	{
		public static Uris Default { get; } = new Uris();

		Uris() : base(Start.A.Selection<string>().AndOf<Uri>().By.Instantiation) {}
	}
}
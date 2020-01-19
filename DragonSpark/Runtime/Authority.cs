using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Runtime
{
	public sealed class Authority : Select<Uri, string>
	{
		public static Authority Default { get; } = new Authority();

		Authority() : base(x => x.GetLeftPart(UriPartial.Authority)) {}
	}
}
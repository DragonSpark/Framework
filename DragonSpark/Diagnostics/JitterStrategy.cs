using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Diagnostics;

class JitterStrategy : Select<int, TimeSpan>
{
	protected JitterStrategy(ISelect<int, TimeSpan> retry) : base(retry.Then().Select(AddJitter.Default)) {}
}
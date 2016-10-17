using DragonSpark.Sources;
using System;

namespace DragonSpark.Application
{
	public class SuppliedCurrentTime : SuppliedSource<DateTimeOffset>, ICurrentTime
	{
		public SuppliedCurrentTime( DateTimeOffset now ) : base( now ) {}
	}
}
using System;
using DragonSpark.ComponentModel;

namespace DragonSpark.Testing.TestObjects
{
	public class ObjectCreationTarget
	{
		[NewGuidDefaultValue]
		public Guid ID { get; set; }

		[IoCDefault]
		public DragonSparkObject DragonSparkObject { get; set; }

        [IoCDefault( "Instance" )]
		public DragonSparkObject Instance { get; set; }
	}

	public class DragonSparkSimpleObject
	{}
}
using System;

namespace DragonSpark.Testing.TestObjects.Synchronization
{
	public class EntityWithSimilarProperties
	{
		public Guid ID { get; set; }
		public string Name { get; set; }
		public string ShortDescription { get; set; }
		internal string Comments { get; set; }

	}
}
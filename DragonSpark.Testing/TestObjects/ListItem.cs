using System;

namespace DragonSpark.Testing.TestObjects
{
	public class ListItem : IEquatable<ListItem>
	{
		public Guid ID { get; set; }

		public bool Equals( ListItem other )
		{
			var result = other.ID == ID;
			return result;
		}
	}
}
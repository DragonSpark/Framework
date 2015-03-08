using System;

namespace DragonSpark.Testing.TestObjects.Synchronization
{
	public class Entity
	{
		public Guid ID { get; set; }

		public string Name { get; set; }
	
		public DateTime Date { get; set; }
		
		public string Description { get; set; }
		
		public string IgnoreDisplayName { get; set; }
		
		public string Comments { get; set; }
		
		public string DescriptionFull { get; set; }

		public EntityComplexProperty ComplexProperty { get; set; }

		public Entity Child
		{
			get { return child ?? ( child = new Entity() ); }
			set { child = value; }
		}	Entity child;
	}

	public class EntityComplexProperty
	{
		public Guid ID { get; set; }

		public string Name { get; set; }
	}
}
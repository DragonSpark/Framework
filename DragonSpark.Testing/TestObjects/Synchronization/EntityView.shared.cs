using System;
using DragonSpark.Objects.Synchronization;

namespace DragonSpark.Testing.TestObjects.Synchronization
{
	public class EntityView
	{
		public Guid ID { get; set; }
		public string Name { get; set; }
		public string Date { get; set; }
		[Synchronization( typeof(TestObjects.Synchronization.Entity), "Child.Description" )]
		public string ChildDescription { get; set; }
		public string CommentsField { get; set; }
		public string IgnoreDisplayName { get; set; }
		public string Description { get; set; }
		public string ChildName { get; set; }

		public EntityViewComplexProperty ComplexPropertyView { get; set; }
	}

	public class EntityViewComplexProperty
	{
		public Guid ID { get; set; }

		public string Name { get; set; }
	}

}
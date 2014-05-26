using System;

namespace DragonSpark.Application.Communication.Entity.Notifications
{
	[AttributeUsage( AttributeTargets.Class )]
	public sealed class NotificationProcessorAttribute : Attribute
	{
		readonly Type processorType;
		readonly string name;

		public NotificationProcessorAttribute( Type processorType ) : this( processorType, null )
		{}

		public NotificationProcessorAttribute( Type processorType, string name )
		{
			this.processorType = processorType;
			this.name = name;
		}

		public Type ProcessorType
		{
			get { return processorType; }
		}

		public string Name
		{
			get { return name; }
		}
	}
}
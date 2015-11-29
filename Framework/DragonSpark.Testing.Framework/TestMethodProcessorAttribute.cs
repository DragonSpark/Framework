using System;

namespace DragonSpark.Testing.Framework
{
	[AttributeUsage( AttributeTargets.Method )]
	public abstract class TestMethodProcessorAttribute : Attribute
	{
		protected TestMethodProcessorAttribute()
		{
			Priority = Priority.Normal;
		}

		public Priority Priority { get; set; }

		protected internal abstract void Process( TestMethodProcessingContext context );
	}
}
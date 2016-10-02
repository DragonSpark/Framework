namespace DragonSpark
{
	class DefaultPriorityAware : IPriorityAware
	{
		public static DefaultPriorityAware Default { get; } = new DefaultPriorityAware();
		DefaultPriorityAware() : this( Priority.Normal ) {}

		public DefaultPriorityAware( Priority priority )
		{
			Priority = priority;
		}

		public Priority Priority { get; }
	}
}
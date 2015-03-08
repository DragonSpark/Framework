namespace DragonSpark.Application.Logging
{
    public class LoggingPriorityMapping
    {
        public Priority Priority { get; set; }
		
        public Microsoft.Practices.Prism.Logging.Priority LoggingPriority { get; set; }
    }
}
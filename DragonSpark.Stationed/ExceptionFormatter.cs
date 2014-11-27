namespace DragonSpark.Stationed
{
	public class ExceptionFormatter : DragonSpark.Diagnostics.ExceptionFormatter
	{
		public ExceptionFormatter( ApplicationDetails details ) : base( new EnterpriseLibraryExceptionFormatter(), details )
		{}
	}
}
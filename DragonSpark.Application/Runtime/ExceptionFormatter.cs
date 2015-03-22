namespace DragonSpark.Application.Runtime
{
	public class ExceptionFormatter : Diagnostics.ExceptionFormatter
	{
		public ExceptionFormatter( ApplicationInformation information ) : base( new EnterpriseLibraryExceptionFormatter(), information )
		{}
	}
}
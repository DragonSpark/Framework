namespace DragonSpark.Windows.Runtime
{
	public class ApplicationExceptionFormatter : Diagnostics.ApplicationExceptionFormatter
	{
		public ApplicationExceptionFormatter( ApplicationInformation information ) : base( new EnterpriseLibraryExceptionFormatter(), information )
		{}
	}
}
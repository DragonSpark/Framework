namespace DragonSpark.Application.Runtime
{
	public class ExceptionFormatter : Diagnostics.ExceptionFormatter
	{
		public ExceptionFormatter( ApplicationProfile profile ) : base( new EnterpriseLibraryExceptionFormatter(), profile )
		{}
	}
}
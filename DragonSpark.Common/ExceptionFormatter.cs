namespace DragonSpark.Common
{
	public class ExceptionFormatter : DragonSpark.Diagnostics.ExceptionFormatter
	{
		public ExceptionFormatter( ApplicationProfile profile ) : base( new EnterpriseLibraryExceptionFormatter(), profile )
		{}
	}
}
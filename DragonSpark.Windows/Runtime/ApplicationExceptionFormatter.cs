namespace DragonSpark.Windows.Runtime
{
	public class ApplicationExceptionFormatter : Diagnostics.ApplicationExceptionFormatter
	{
		public ApplicationExceptionFormatter( AssemblyInformation information ) : base( EnterpriseLibraryExceptionFormatter.Instance, information )
		{}
	}
}
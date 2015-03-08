using System;

namespace DragonSpark.Application.Assemblies
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments"), AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class OriginalLaunchDateAttribute : Attribute
	{
		readonly DateTime launchDate;
		
		public OriginalLaunchDateAttribute( int year, int month, int day )
		{
			launchDate = new DateTime( year, month, day );
		}

		public DateTime LaunchDate
		{
			get { return launchDate; }
		}
	}
}
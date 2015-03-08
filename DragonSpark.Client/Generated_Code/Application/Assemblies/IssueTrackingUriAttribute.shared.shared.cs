using System;

namespace DragonSpark.Application.Assemblies
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments", Justification = "Property is converted to a System.Uri object." ), AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class IssueTrackingUriAttribute : Attribute
	{
		readonly Uri uri;

		public IssueTrackingUriAttribute( string uri )
		{
			this.uri = new Uri( uri );
		}

		public Uri Uri
		{
			get { return uri; }
		}
	}
}
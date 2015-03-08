using System;

namespace DragonSpark.Application.Assemblies
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments", Justification = "Property is converted to a System.Uri object." ), AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class SupportUriAttribute : Attribute
	{
		readonly Uri uri;

		public SupportUriAttribute( string uri )
		{
			this.uri = new Uri( uri );
		}

		public Uri Uri
		{
			get { return uri; }
		}
	}
}